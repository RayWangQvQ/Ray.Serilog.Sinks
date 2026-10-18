#!/usr/bin/env bash
# Write a new CHANGELOG section to the top of CHANGELOG.md. Re-run safe: replaces
# the top section if it already matches this version.
#
#   update-changelog.sh <version> [--since <tag>] [--from-file <file>]
set -euo pipefail

script_dir=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
repo_dir=${VERSION_REPO_DIR:-$(cd "$script_dir/.." && pwd)}
changelog="$repo_dir/CHANGELOG.md"
die() { echo "$1" >&2; exit 1; }

version=${1:-}; [ -n "$version" ] || die "Usage: $0 <version> [--since <tag>] [--from-file <file>]"
shift

extra_args=()
while [ $# -gt 0 ]; do
    case "$1" in
    --since | --from-file) extra_args+=("$1" "$2"); shift 2 ;;
    *) die "unknown arg: $1" ;;
    esac
done

[ -f "$changelog" ] || die "$changelog not found"

section=$(VERSION_REPO_DIR="$repo_dir" bash "$script_dir/release-notes.sh" section "$version" "${extra_args[@]}")

first=$(grep -n '^## ' "$changelog" | head -1 | cut -d: -f1 || true)
second=$(grep -n '^## ' "$changelog" | sed -n 2p | cut -d: -f1 || true)

tmp=$(mktemp)
if [ -n "$first" ] && [ "$(sed -n "${first}p" "$changelog")" = "## $version" ]; then
    { echo "$section"; [ -n "$second" ] && tail -n "+$second" "$changelog"; } > "$tmp"
    echo "replaced existing top section for $version"
else
    { echo "$section"; cat "$changelog"; } > "$tmp"
    echo "inserted new section for $version at top"
fi
mv "$tmp" "$changelog"
