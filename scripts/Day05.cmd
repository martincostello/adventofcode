@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 5 "%~dp0\..\input\day-05\input.txt" 1 && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 5 "%~dp0\..\input\day-05\input.txt" 2 && echo.
