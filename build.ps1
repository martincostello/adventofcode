param(
    [Parameter(Mandatory = $false)][string] $Configuration = "Release",
    [Parameter(Mandatory = $false)][string] $VersionSuffix = "",
    [Parameter(Mandatory = $false)][string] $OutputPath = "",
    [Parameter(Mandatory = $false)][switch] $SkipTests
)

$ErrorActionPreference = "Stop"

$solutionPath = Split-Path $MyInvocation.MyCommand.Definition
$sdkFile = Join-Path $solutionPath "global.json"

$testProjects = @(
    (Join-Path $solutionPath "tests\AdventOfCode.Tests\AdventOfCode.Tests.csproj")
)

$dotnetVersion = (Get-Content $sdkFile | Out-String | ConvertFrom-Json).sdk.version

if ($OutputPath -eq "") {
    $OutputPath = Join-Path "$(Convert-Path "$PSScriptRoot")" "artifacts"
}

$installDotNetSdk = $false;

if (($null -eq (Get-Command "dotnet.exe" -ErrorAction SilentlyContinue)) -and ($null -eq (Get-Command "dotnet" -ErrorAction SilentlyContinue))) {
    Write-Host "The .NET Core SDK is not installed."
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
        Write-Host "The required version of the .NET Core SDK is not installed. Expected $dotnetVersion."
        $installDotNetSdk = $true
    }
}

if ($installDotNetSdk -eq $true) {
    $env:DOTNET_INSTALL_DIR = Join-Path "$(Convert-Path "$PSScriptRoot")" ".dotnetcli"
    $sdkPath = Join-Path $env:DOTNET_INSTALL_DIR "sdk\$dotnetVersion"

    if (!(Test-Path $sdkPath)) {
        if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
            mkdir $env:DOTNET_INSTALL_DIR | Out-Null
        }
        $installScript = Join-Path $env:DOTNET_INSTALL_DIR "install.ps1"
        Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile $installScript -UseBasicParsing
        & $installScript -Version "$dotnetVersion" -InstallDir "$env:DOTNET_INSTALL_DIR" -NoPath
    }

    $env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"
    $dotnet = Join-Path "$env:DOTNET_INSTALL_DIR" "dotnet.exe"
}
else {
    $dotnet = "dotnet.exe"
}

Write-Host "Building solution..." -ForegroundColor Green

& $dotnet build ./AdventOfCode.sln --output $OutputPath --configuration $Configuration

if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed with exit code $LASTEXITCODE"
}

if ($SkipTests -eq $false) {
    Write-Host "Running tests..." -ForegroundColor Green
    ForEach ($testProject in $testProjects) {

        & $dotnet test $testProject --output $OutputPath --configuration $Configuration

        if ($LASTEXITCODE -ne 0) {
            throw "dotnet test failed with exit code $LASTEXITCODE"
        }
    }
}
