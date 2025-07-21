# Unit Test Coverage Generation Script (PowerShell)
# Usage: .\scripts\generate-coverage.ps1 [Parameters]
# Parameters:
#   -Threshold <number>       Coverage threshold (default: 90)
#   -NoThresholdCheck         Skip coverage threshold check
#   -OutputDir <path>         Coverage report output directory (default: CoverageReport)
#   -TestResultsDir <path>    Test results directory (default: TestResults)
#   -Help                     Show help information

param(
    [int]$Threshold = 90,
    [switch]$NoThresholdCheck,
    [string]$OutputDir = "CoverageReport",
    [string]$TestResultsDir = "TestResults",
    [switch]$Help
)

# Script root directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$ProjectRoot = Split-Path -Parent $ScriptDir

# Color definitions
$Colors = @{
    Red = "Red"
    Green = "Green"
    Yellow = "Yellow"
    Blue = "Blue"
    Cyan = "Cyan"
}

# Help information
function Show-Help {
    Write-Host "Unit Test Coverage Generation Script (PowerShell)" -ForegroundColor $Colors.Blue
    Write-Host ""
    Write-Host "Usage: .\scripts\generate-coverage.ps1 [Parameters]"
    Write-Host ""
    Write-Host "Parameters:"
    Write-Host "  -Threshold <number>       Coverage threshold (default: 90)"
    Write-Host "  -NoThresholdCheck         Skip coverage threshold check"
    Write-Host "  -OutputDir <path>         Coverage report output directory (default: CoverageReport)"
    Write-Host "  -TestResultsDir <path>    Test results directory (default: TestResults)"
    Write-Host "  -Help                     Show help information"
    Write-Host ""
    Write-Host "Examples:"
    Write-Host "  .\scripts\generate-coverage.ps1                           # Use default settings"
    Write-Host "  .\scripts\generate-coverage.ps1 -Threshold 80             # Set coverage threshold to 80%"
    Write-Host "  .\scripts\generate-coverage.ps1 -NoThresholdCheck         # Skip threshold check"
    Write-Host "  .\scripts\generate-coverage.ps1 -OutputDir MyReport       # Custom output directory"
}

# Logging functions
function Write-InfoLog {
    param([string]$Message)
    Write-Host "INFO: $Message" -ForegroundColor $Colors.Blue
}

function Write-SuccessLog {
    param([string]$Message)
    Write-Host "SUCCESS: $Message" -ForegroundColor $Colors.Green
}

function Write-WarningLog {
    param([string]$Message)
    Write-Host "WARNING: $Message" -ForegroundColor $Colors.Yellow
}

function Write-ErrorLog {
    param([string]$Message)
    Write-Host "ERROR: $Message" -ForegroundColor $Colors.Red
}

# Check if tool is installed
function Test-Tool {
    param([string]$ToolName)

    try {
        $null = Get-Command $ToolName -ErrorAction Stop
        return $true
    }
    catch {
        return $false
    }
}

# Install ReportGenerator
function Install-ReportGenerator {
    Write-InfoLog "Checking ReportGenerator installation status..."

    try {
        $toolList = dotnet tool list -g 2>$null
        if ($toolList -match "dotnet-reportgenerator-globaltool") {
            Write-InfoLog "ReportGenerator is already installed"
        }
        else {
            Write-InfoLog "Installing ReportGenerator..."
            dotnet tool install -g dotnet-reportgenerator-globaltool
            if ($LASTEXITCODE -eq 0) {
                Write-SuccessLog "ReportGenerator installation completed"
            }
            else {
                Write-ErrorLog "ReportGenerator installation failed"
                exit 1
            }
        }
    }
    catch {
        Write-ErrorLog "Error checking or installing ReportGenerator: $_"
        exit 1
    }
}

# Clean old files
function Clear-OldFiles {
    Write-InfoLog "Cleaning old test results and coverage reports..."

    $testResultsPath = Join-Path $ProjectRoot $TestResultsDir
    $outputPath = Join-Path $ProjectRoot $OutputDir

    if (Test-Path $testResultsPath) {
        Remove-Item -Path $testResultsPath -Recurse -Force
    }

    if (Test-Path $outputPath) {
        Remove-Item -Path $outputPath -Recurse -Force
    }
}

# Run unit tests and generate coverage data
function Invoke-Tests {
    Write-InfoLog "Running unit tests and generating coverage data..."

    Set-Location $ProjectRoot

    $testArgs = @(
        "test"
        "--configuration", "Release"
        "--verbosity", "normal"
        "--collect:XPlat Code Coverage"
        "--results-directory", "./$TestResultsDir/"
        "--logger", "trx"
        "--logger", "console;verbosity=detailed"
        "--"
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura"
    )

    & dotnet @testArgs

    if ($LASTEXITCODE -ne 0) {
        Write-ErrorLog "Unit tests failed!"
        exit 1
    }

    Write-SuccessLog "Unit tests completed!"

    # List generated coverage files
    Write-InfoLog "Generated coverage files:"
    Get-ChildItem -Path "./$TestResultsDir" -Recurse -Include "*.xml", "coverage.*" | Select-Object -First 10 | ForEach-Object {
        Write-Host "  $($_.FullName)"
    }
}

# Find coverage files
function Find-CoverageFiles {
    $testResultsPath = Join-Path $ProjectRoot $TestResultsDir

    # First priority: cobertura format files
    $coberturaFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "*.cobertura.xml" -ErrorAction SilentlyContinue
    if ($coberturaFiles) {
        Write-InfoLog "Found cobertura format coverage files"
        return ($coberturaFiles.FullName -join ";")
    }

    # Second priority: opencover format files
    $opencoverFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "*.opencover.xml" -ErrorAction SilentlyContinue
    if ($opencoverFiles) {
        Write-InfoLog "Found opencover format coverage files"
        return ($opencoverFiles.FullName -join ";")
    }

    # Third priority: generic coverage.xml files
    $coverageFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "coverage.xml" -ErrorAction SilentlyContinue
    if ($coverageFiles) {
        Write-InfoLog "Found generic coverage.xml files"
        return ($coverageFiles.FullName -join ";")
    }

    Write-ErrorLog "No coverage files found!"
    exit 1
}

# Generate coverage report
function New-CoverageReport {
    Write-InfoLog "Generating coverage report..."

    $coverageFiles = Find-CoverageFiles
    Write-InfoLog "Using coverage files: $coverageFiles"

    $reportArgs = @(
        "-reports:$coverageFiles"
        "-targetdir:$OutputDir"
        "-reporttypes:Html;Cobertura;JsonSummary;TextSummary;Badges"
        "-verbosity:Info"
    )

    & reportgenerator @reportArgs

    if ($LASTEXITCODE -eq 0) {
        Write-SuccessLog "Coverage report generation completed!"
    }
    else {
        Write-ErrorLog "Coverage report generation failed!"
        exit 1
    }
}

# Show coverage details
function Show-CoverageDetails {
    Write-InfoLog "Detailed coverage analysis:"
    Write-Host "=============================="

    # Show overall summary
    $summaryPath = Join-Path $ProjectRoot "$OutputDir/Summary.txt"
    if (Test-Path $summaryPath) {
        Write-Host ""
        Write-InfoLog "Overall coverage summary:"
        Get-Content $summaryPath
        Write-Host ""
    }

    # Parse and display class-level coverage
    $jsonPath = Join-Path $ProjectRoot "$OutputDir/Summary.json"
    if (Test-Path $jsonPath) {
        Write-Host ""
        Write-InfoLog "Class-level coverage details:"
        Write-Host "--------------------------------"

        try {
            $summary = Get-Content $jsonPath | ConvertFrom-Json
            $assembly = $summary.coverage.assemblies | Where-Object { $_.name -like "*Ray.*" }

            if ($assembly -and $assembly.classesinassembly) {
                # Show all class coverage
                $assembly.classesinassembly | Where-Object { $_.name -ne "" } | ForEach-Object {
                    $branchCoverage = if ($_.branchcoverage -eq $null) { "N/A" } else { "$($_.branchcoverage)%" }
                    Write-Host "Class: $($_.name) | Line Coverage: $($_.coverage)% | Branch Coverage: $branchCoverage | Lines: $($_.coveredlines)/$($_.coverablelines)"
                }

                Write-Host ""
                Write-WarningLog "Low coverage classes (< 50%):"
                Write-Host "------------------------------------"

                $lowCoverageClasses = $assembly.classesinassembly | Where-Object {
                    $_.name -ne "" -and $_.coverage -is [double] -and $_.coverage -lt 50
                }

                if ($lowCoverageClasses) {
                    $lowCoverageClasses | ForEach-Object {
                        Write-Host "- $($_.name): $($_.coverage)% ($($_.coveredlines)/$($_.coverablelines) lines)" -ForegroundColor $Colors.Red
                    }
                }
                else {
                    Write-Host "No low coverage classes" -ForegroundColor $Colors.Green
                }

                Write-Host ""
                Write-SuccessLog "High coverage classes (>= 80%):"
                Write-Host "--------------------------------------"

                $highCoverageClasses = $assembly.classesinassembly | Where-Object {
                    $_.name -ne "" -and $_.coverage -is [double] -and $_.coverage -ge 80
                }

                if ($highCoverageClasses) {
                    $highCoverageClasses | ForEach-Object {
                        Write-Host "- $($_.name): $($_.coverage)% ($($_.coveredlines)/$($_.coverablelines) lines)" -ForegroundColor $Colors.Green
                    }
                }
                else {
                    Write-Host "No high coverage classes"
                }
            }
        }
        catch {
            Write-WarningLog "Error parsing coverage JSON file, cannot display detailed information: $_"
        }
    }
}

# Check coverage threshold
function Test-CoverageThreshold {
    if ($NoThresholdCheck) {
        Write-InfoLog "Skipping coverage threshold check"
        return $true
    }

    Write-InfoLog "Checking coverage threshold..."

    $jsonPath = Join-Path $ProjectRoot "$OutputDir/Summary.json"
    if (Test-Path $jsonPath) {
        try {
            $summary = Get-Content $jsonPath | ConvertFrom-Json
            $coverage = $summary.summary.linecoverage
            Write-InfoLog "Current coverage: $coverage%"

            if ($coverage -ge $Threshold) {
                Write-SuccessLog "Coverage check passed! ($coverage% >= $Threshold%)"
                return $true
            }
            else {
                Write-ErrorLog "Coverage check failed! Current: $coverage%, Required: $Threshold%"
                return $false
            }
        }
        catch {
            Write-WarningLog "Error parsing coverage data, cannot check threshold: $_"
            return $true
        }
    }
    else {
        Write-ErrorLog "Coverage report file not found!"
        return $false
    }
}

# Main function
function Main {
    if ($Help) {
        Show-Help
        return
    }

    Write-Host "========================================" -ForegroundColor $Colors.Cyan
    Write-InfoLog "Starting unit test coverage report generation"
    Write-Host "========================================" -ForegroundColor $Colors.Cyan
    Write-Host ""

    Write-InfoLog "Configuration:"
    Write-Host "  Project root: $ProjectRoot"
    Write-Host "  Test results directory: $TestResultsDir"
    Write-Host "  Coverage report directory: $OutputDir"
    Write-Host "  Coverage threshold: $Threshold%"
    Write-Host "  Check threshold: $(-not $NoThresholdCheck)"
    Write-Host ""

    # Check required tools
    if (-not (Test-Tool "dotnet")) {
        Write-ErrorLog ".NET SDK is not installed, please install .NET SDK first"
        exit 1
    }

    # Install ReportGenerator
    Install-ReportGenerator

    # Clean old files
    Clear-OldFiles

    # Run tests
    Invoke-Tests

    # Generate report
    New-CoverageReport

    # Show details
    Show-CoverageDetails

    # Check threshold
    $thresholdPassed = Test-CoverageThreshold

    Write-Host ""
    Write-Host "========================================" -ForegroundColor $Colors.Cyan

    $reportPath = Join-Path $ProjectRoot "$OutputDir/index.html"

    if ($thresholdPassed) {
        Write-SuccessLog "Coverage report generation completed!"
        Write-InfoLog "Report location: $reportPath"

        # Ask if user wants to open the report
        $openReport = Read-Host "Open coverage report? (y/N)"
        if ($openReport -eq "y" -or $openReport -eq "Y") {
            if (Test-Path $reportPath) {
                Start-Process $reportPath
            }
        }
    }
    else {
        Write-ErrorLog "Coverage report generated, but threshold requirement not met!"
        Write-InfoLog "Report location: $reportPath"
        exit 1
    }

    Write-Host "========================================" -ForegroundColor $Colors.Cyan
}

# Set error handling
$ErrorActionPreference = "Stop"

# Run main function
try {
    Main
}
catch {
    Write-ErrorLog "Error occurred during script execution: $_"
    exit 1
}
