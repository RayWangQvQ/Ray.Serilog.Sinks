# Set strict mode
Set-StrictMode -Version Latest

# Change directory to bin/nupkgs
Set-Location ../bin/nupkgs

# List the files in the current directory
Get-ChildItem

# Define the api_key and source_url variables
$api_key = ""
$source_url = "https://api.nuget.org/v3/index.json"

# Prompt for api_key if it is empty
if ([string]::IsNullOrEmpty($api_key)) {
    $api_key = Read-Host "api_key"
}
else {
    Write-Host "api_key: $api_key"
}

# Push the nupkg files to the source_url with the api_key
dotnet nuget push "*.nupkg" --skip-duplicate --api-key $api_key --source $source_url