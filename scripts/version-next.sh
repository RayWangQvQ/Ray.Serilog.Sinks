#!/usr/bin/env bash
# Version derivation from git tag history (CI-managed versioning). No file is the
# source of truth; local builds stay 0.0.0-dev, CI injects the version explicitly.
#
#   alpha           -> <next-patch-of-latest-stable>-alpha.N, N = commits since that tag
#   stable <level>  -> bump latest stable tag by patch|minor|major
#   latest          -> latest stable tag (empty if none)
#
# Stable tag = bare x.y.z (no prerelease). A leading `v` prefix is tolerated and
# stripped (this repo has legacy `v*` tags); new releases use bare tags. CI needs
# fetch-depth: 0.
set -euo pipefail

repo_dir=${VERSION_REPO_DIR:-$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)}

die() { echo "$1" >&2; exit 1; }

# List stable tags as bare x.y.z (strips an optional leading `v`).
stable_tags() {
    git -C "$repo_dir" tag -l \
        | grep -E '^v?[0-9]+\.[0-9]+\.[0-9]+$' \
        | sed -E 's/^v//' \
        | sort -V || true
}
latest_stable() { stable_tags | tail -1; }

# Map a bare version back to the real tag name in the repo (bare preferred, else
# the `v`-prefixed one). Used for git range lookups that need an actual ref.
raw_tag() { # <version>
    local ver=$1
    if git -C "$repo_dir" rev-parse -q --verify "refs/tags/$ver" >/dev/null; then
        echo "$ver"
    elif git -C "$repo_dir" rev-parse -q --verify "refs/tags/v$ver" >/dev/null; then
        echo "v$ver"
    else
        die "tag for $ver not found"
    fi
}

# First-release fallback: version line starts at 0.1.0.
default_base="0.1.0"

bump() { # <version> <patch|minor|major>
    awk -F. -v level="$2" '{
        if (level == "major")      printf "%d.0.0", $1 + 1
        else if (level == "minor") printf "%d.%d.0", $1, $2 + 1
        else                       printf "%d.%d.%d", $1, $2, $3 + 1
    }' <<< "$1"
}

alpha() {
    local latest base n
    latest=$(latest_stable)
    if [ -z "$latest" ]; then
        base=$default_base
        n=$(git -C "$repo_dir" rev-list --count HEAD)
    else
        base=$(bump "$latest" patch)
        n=$(git -C "$repo_dir" rev-list --count "$(raw_tag "$latest")..HEAD")
    fi
    # squash merge keeps N == number of merged PRs; resets after a stable release
    [ "$n" -ge 1 ] || die "no commits since ${latest:-repo start}, nothing to alpha"
    echo "$base-alpha.$n"
}

stable() {
    local level=${1:-} latest
    case "$level" in
    patch | minor | major) ;;
    *) die "unknown level: '$level' (want patch|minor|major)" ;;
    esac
    latest=$(latest_stable)
    if [ -z "$latest" ]; then echo "$default_base"; else bump "$latest" "$level"; fi
}

case "${1:-help}" in
alpha) alpha ;;
stable) stable "${2:-}" ;;
latest) latest_stable ;;
*)
    cat <<EOF
Usage: $0 {alpha|stable <patch|minor|major>|latest}
Env VERSION_REPO_DIR overrides the target git repo (tests).
EOF
    ;;
esac
