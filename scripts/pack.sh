#!/usr/bin/env bash
set -e
set -u
set -o pipefail

cd ..
mkdir -p ./bin/nupkgs
rm -rf ./bin/nupkgs/*
dotnet pack --output bin/nupkgs