@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 9 "%~dp0\..\input\day-09\input.txt" && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 9 "%~dp0\..\input\day-09\input.txt" true && echo.
