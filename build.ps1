[CmdletBinding()]
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$projectRoot = $PSScriptRoot
$dotnetHome = Join-Path $projectRoot ".dotnet-cli-home"
$solutionPath = Join-Path $projectRoot "CommandManager.sln"

if (-not (Test-Path -LiteralPath $dotnetHome)) {
    New-Item -ItemType Directory -Path $dotnetHome | Out-Null
}

$env:DOTNET_CLI_HOME = $dotnetHome

dotnet build $solutionPath --configuration $Configuration

if ($LASTEXITCODE -ne 0) {
    throw "Command Manager build failed with exit code $LASTEXITCODE."
}

$outputPath = Join-Path $projectRoot "src\CreatorForge.CommandManager\bin\$Configuration\CreatorForge.CommandManager.dll"
if (-not (Test-Path -LiteralPath $outputPath)) {
    throw "Build reported success, but the expected DLL was not found: $outputPath"
}

Write-Host "Build succeeded: $outputPath"

