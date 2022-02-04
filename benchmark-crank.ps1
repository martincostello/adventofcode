#! /usr/bin/env pwsh

#Requires -PSEdition Core
#Requires -Version 7

param(
    [Parameter(Mandatory = $true)][string] $Benchmark,
    [Parameter(Mandatory = $true)][string] $Repository,
    [Parameter(Mandatory = $true)][string] $PullRequestId,
    [Parameter(Mandatory = $true)][string] $AccessToken
)

if ($IsWindows) {
    Start-Process -FilePath "crank-agent" -WindowStyle Hidden -PassThru
} else {
    Start-Process -FilePath "crank-agent" -PassThru
}

Start-Sleep -Seconds 2

$repoPath = Split-Path $MyInvocation.MyCommand.Definition
$components = "app"
$config = Join-Path $repoPath "benchmark.yml"
$profiles = "local"
$publishResults = $true

crank-pr `
    --access-token $AccessToken `
    --benchmarks $Benchmark `
    --components $components `
    --config $config `
    --profiles $profiles `
    --publish-results $publishResults `
    --pull-request $PullRequestId `
    --repository $Repository
