#! /usr/bin/env pwsh

#Requires -PSEdition Core
#Requires -Version 7

param(
    [Parameter(Mandatory = $true)][string] $PullRequestId,
    [Parameter(Mandatory = $false)][string] $AccessToken,
    [Parameter(Mandatory = $false)][string] $Benchmark = "root",
    [Parameter(Mandatory = $false)][string] $Repository = "https://github.com/martincostello/adventofcode",
    [Parameter(Mandatory = $false)][switch] $PublishResults
)

if ($IsWindows) {
    $agent = Start-Process -FilePath "dotnet" -ArgumentList @("crank-agent") -PassThru -WindowStyle Hidden
} else {
    $agent = Start-Process -FilePath "dotnet" -ArgumentList @("crank-agent") -PassThru
}

Start-Sleep -Seconds 2

$repoPath = $PSScriptRoot
$components = "app"
$config = Join-Path $repoPath "benchmark.yml"
$profiles = "local"

$additionalArgs = @()

if (![string]::IsNullOrEmpty($AccessToken)) {
    $additionalArgs += "--access-token"
    $additionalArgs += $AccessToken

    if ($PublishResults) {
        $additionalArgs += "--publish-results"
        $additionalArgs += "true"
    }
}

try {
    & dotnet crank-pr `
        --benchmarks $Benchmark `
        --components $components `
        --config $config `
        --profiles $profiles `
        --pull-request $PullRequestId `
        --repository $Repository `
        $additionalArgs
}
finally {
    Stop-Process -InputObject $agent -Force | Out-Null
}

if ($LASTEXITCODE -ne 0) {
    throw "crank-pr failed with exit code $LASTEXITCODE"
}
