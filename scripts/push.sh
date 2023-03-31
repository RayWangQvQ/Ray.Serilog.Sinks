#!/usr/bin/env bash
set -e
set -u
set -o pipefail

cd ../bin/nupkgs
ls

api_key=""
source_url="https://api.nuget.org/v3/index.json"

if [ -z "$api_key" ]; then
    read -p "api_key:" api_key
else
    say "api_key: $api_key"
fi

dotnet nuget push "*.nupkg" --skip-duplicate --api-key $api_key --source $source_url
