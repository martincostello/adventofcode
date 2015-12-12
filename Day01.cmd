@echo off

call "%~dp0\Build.cmd" %* && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 1 "%~dp0\input\day-01\input.txt" && echo.
