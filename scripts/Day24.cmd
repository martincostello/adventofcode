@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 24 "%~dp0\..\input\day-24\input.txt" && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 24 "%~dp0\..\input\day-24\input.txt" 4 && echo.
