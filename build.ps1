#! /usr/bin/env pwsh

#Requires -PSEdition Core
#Requires -Version 7

param(
    [Parameter(Mandatory = $false)][string] $Configuration = "Release",
    [Parameter(Mandatory = $false)][switch] $SkipPublish,
    [Parameter(Mandatory = $false)][switch] $SkipTests,
    [Parameter(Mandatory = $false)][string] $Runtime = ""
)

$ErrorActionPreference = "Stop"
$global:ProgressPreference = "SilentlyContinue"

$solutionPath = $PSScriptRoot
$sdkFile = Join-Path $solutionPath "global.json"

$dotnetVersion = (Get-Content $sdkFile | Out-String | ConvertFrom-Json).sdk.version
$installDotNetSdk = $false;

if (($null -eq (Get-Command "dotnet" -ErrorAction SilentlyContinue)) -and ($null -eq (Get-Command "dotnet.exe" -ErrorAction SilentlyContinue))) {
    Write-Host "The .NET SDK is not installed."
    $installDotNetSdk = $true
}
else {
    Try {
        $installedDotNetVersion = (dotnet --version 2>&1 | Out-String).Trim()
    }
    Catch {
        $installedDotNetVersion = "?"
    }

    if ($installedDotNetVersion -ne $dotnetVersion) {
        Write-Host "The required version of the .NET SDK is not installed. Expected $dotnetVersion."
        $installDotNetSdk = $true
    }
}

if ($installDotNetSdk -eq $true) {

    $env:DOTNET_INSTALL_DIR = Join-Path $PSScriptRoot ".dotnet"
    $sdkPath = Join-Path $env:DOTNET_INSTALL_DIR "sdk" $dotnetVersion

    if (!(Test-Path $sdkPath)) {
        if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
            mkdir $env:DOTNET_INSTALL_DIR | Out-Null
        }
        [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor "Tls12"

        if (($PSVersionTable.PSVersion.Major -ge 6) -And !$IsWindows) {
            $installScript = Join-Path $env:DOTNET_INSTALL_DIR "install.sh"
            Invoke-WebRequest "https://dot.net/v1/dotnet-install.sh" -OutFile $installScript -UseBasicParsing
            chmod +x $installScript
            & $installScript --version "$dotnetVersion" --install-dir "$env:DOTNET_INSTALL_DIR" --no-path
        }
        else {
            $installScript = Join-Path $env:DOTNET_INSTALL_DIR "install.ps1"
            Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile $installScript -UseBasicParsing
            & $installScript -Version "$dotnetVersion" -InstallDir "$env:DOTNET_INSTALL_DIR" -NoPath
        }
    }
}
else {
    $env:DOTNET_INSTALL_DIR = Split-Path -Path (Get-Command dotnet).Path
}

$dotnet = Join-Path "$env:DOTNET_INSTALL_DIR" "dotnet"

if ($installDotNetSdk -eq $true) {
    $env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"
}

Write-Host "Building solution..." -ForegroundColor Green

& $dotnet build ./AdventOfCode.sln

if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed with exit code $LASTEXITCODE"
}

if ($SkipTests -eq $false) {
    Write-Host "Running tests..." -ForegroundColor Green

    $additionalArgs = @()

    if (![string]::IsNullOrEmpty($env:GITHUB_SHA)) {
        $additionalArgs += "--logger"
        $additionalArgs += "GitHubActions;report-warnings=false"
    }

    $testProjects = @(
        (Join-Path $solutionPath "tests" "AdventOfCode.Tests" "AdventOfCode.Tests.csproj")
    )

    ForEach ($testProject in $testProjects) {

        & $dotnet test $testProject --configuration $Configuration $additionalArgs

        if ($LASTEXITCODE -ne 0) {
            throw "dotnet test failed with exit code $LASTEXITCODE"
        }
    }
}

if ($SkipPublish -eq $false) {

    Write-Host "Publishing application..." -ForegroundColor Green

    $projectPath = (Join-Path $solutionPath "src" "AdventOfCode.Site")
    $projectFile = Join-Path $projectPath "AdventOfCode.Site.csproj"

    $additionalArgs = @()

    if (![string]::IsNullOrEmpty($Runtime)) {
        $additionalArgs += "--self-contained"
        $additionalArgs += "--runtime"
        $additionalArgs += $Runtime
    }

    & $dotnet publish $projectFile $additionalArgs

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed with exit code $LASTEXITCODE"
    }

    $packageFile = Join-Path $PSScriptRoot "artifacts" "publish" "lambda.zip"

    # Requires that `dotnet tool install --global Amazon.Lambda.Tools` is run first
    dotnet-lambda `
        package `
        --output-package $packageFile `
        --project-location $projectPath

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet-lambda package failed with exit code $LASTEXITCODE"
    }
}

