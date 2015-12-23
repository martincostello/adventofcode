@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 23 "%~dp0\..\input\day-23\input.txt" && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 23 "%~dp0\..\input\day-23\input.txt" 1 && echo.
