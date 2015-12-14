@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 14 "%~dp0\..\input\day-14\input.txt" 2503 && echo.
