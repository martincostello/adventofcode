@echo off

set _MSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_MSBuild% set _MSBuild="%ProgramFiles%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_MSBuild% echo Error: Could not find MSBuild.exe. && exit /b 1

"%~dp0\.nuget\nuget.exe" restore "%~dp0\AdventOfCode.Day10.sln" && echo.
%_MSBuild% "%~dp0\AdventOfCode.Day10.sln" /v:minimal /maxcpucount /nodeReuse:false "/p:OutDir=%~dp0\BuildOutput" %* && echo.
"%~dp0\packages\xunit.runner.console.2.1.0\tools\xunit.console" "%~dp0\BuildOutput\AdventOfCode.Day10.Tests.dll" && echo.
"%~dp0\BuildOutput\AdventOfCode.Day10.exe" 1321131112 40 && echo.
