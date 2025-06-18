#! /usr/bin/env pwsh

#Requires -PSEdition Core
#Requires -Version 7

param(
    [Parameter(Mandatory = $false)][string] $Scenario = "root",
    [Parameter(Mandatory = $false)][string] $Profile = "local",
    [Parameter(Mandatory = $false)][string] $BranchOrCommitOrTag = ""
)

$ErrorActionPreference = "Stop"
$global:ProgressPreference = "SilentlyContinue"

$additionalArgs = @()

if (![string]::IsNullOrEmpty($BranchOrCommitOrTag)) {
    $additionalArgs += "--application.source.branchOrCommit"
    $additionalArgs += $BranchOrCommitOrTag
}

$repoPath = $PSScriptRoot
$config = Join-Path $repoPath "crank.yml"

if ($IsWindows) {
    $agent = Start-Process -FilePath "dotnet" -ArgumentList @("crank-agent") -PassThru -WindowStyle Hidden
} else {
    $agent = Start-Process -FilePath "dotnet" -ArgumentList @("crank-agent") -PassThru
}

Start-Sleep -Seconds 2

try {
    dotnet crank --config $config --scenario $Scenario --profile $Profile $additionalArgs
}
finally {
    Stop-Process -InputObject $agent -Force | Out-Null
}

if ($LASTEXITCODE -ne 0) {
    throw "crank failed with exit code $LASTEXITCODE"
}
