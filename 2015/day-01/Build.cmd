@echo off

set _MSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_MSBuild% set _MSBuild="%ProgramFiles%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_MSBuild% echo Error: Could not find MSBuild.exe. && exit /b 1

%_MSBuild% "%~dp0\AdventOfCode.Day1\AdventOfCode.Day1.sln" /v:minimal /maxcpucount /nodeReuse:false "/p:OutDir=%~dp0\BuildOutput" %* && echo.
"%~dp0\BuildOutput\AdventOfCode.Day1.exe" "%~dp0\input.txt" && echo.
