#!/usr/bin/env bash
# Release notes / CHANGELOG section generator: reads squash-merge PR titles since
# the last stable tag, groups by type prefix, keeps the rest intact.
#
#   notes [--since <tag>] [--from-file <f>]              -> "- <title>" lines
#   section <version> [--since <tag>] [--from-file <f>]  -> "## <version>" + entries
set -euo pipefail

script_dir=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
repo_dir=${VERSION_REPO_DIR:-$(cd "$script_dir/.." && pwd)}
die() { echo "$1" >&2; exit 1; }

# priority: BREAKING > Feature > Fix > 维护 > other (strip leading ** before ranking)
rank_of() {
    local t=$1
    t=$(sed -E 's/^\*+//; s/^[[:space:]]+//' <<< "$t")
    case "$t" in
    BREAKING* | breaking*) echo 0 ;;
    Feature* | feature* | feat*) echo 1 ;;
    Fix* | fix*) echo 2 ;;
    维护*) echo 3 ;;
    *) echo 4 ;;
    esac
}

sort_titles() {
    local i=0 t
    while IFS= read -r t; do
        [ -n "$t" ] || continue
        echo "$(rank_of "$t") $i $t"
        i=$((i + 1))
    done | sort -s -n -k1,1 -k2,2 | cut -d' ' -f3-
}

read_titles() { # <since> <from_file>
    if [ -n "$2" ]; then cat "$2"
    elif [ -n "$1" ]; then
        # `since` is a bare version; map it back to the real tag ref (bare or v-prefixed).
        local since_ref=$1
        if ! git -C "$repo_dir" rev-parse -q --verify "refs/tags/$1" >/dev/null \
             && git -C "$repo_dir" rev-parse -q --verify "refs/tags/v$1" >/dev/null; then
            since_ref="v$1"
        fi
        git -C "$repo_dir" log --pretty=%s "$since_ref..HEAD"
    else git -C "$repo_dir" log --pretty=%s; fi
}

latest_stable() { VERSION_REPO_DIR="$repo_dir" bash "$script_dir/version-next.sh" latest; }

entries() { # <since> <from_file>; empty list -> fail
    local since=$1 from_file=$2 titles
    titles=$(read_titles "$since" "$from_file")
    [ -n "${titles// /}" ] || die "no merged PRs since ${since:-repo start}, refusing empty release"
    sort_titles <<< "$titles" | sed 's/^/- /'
}

cmd=${1:-help}
shift || true
case "$cmd" in
notes | section)
    version=""
    if [ "$cmd" = section ]; then
        version=${1:-}; [ -n "$version" ] || die "section needs a version"; shift
    fi
    since=""; from_file=""
    while [ $# -gt 0 ]; do
        case "$1" in
        --since) since=$2; shift 2 ;;
        --from-file) from_file=$2; shift 2 ;;
        *) die "unknown arg: $1" ;;
        esac
    done
    [ -n "$from_file" ] || since=${since:-$(latest_stable)}
    body=$(entries "$since" "$from_file")
    if [ "$cmd" = section ]; then printf '## %s\n%s\n' "$version" "$body"; else echo "$body"; fi
    ;;
*)
    cat <<EOF
Usage: $0 {notes|section <version>} [--since <tag>] [--from-file <file>]
EOF
    ;;
esac
