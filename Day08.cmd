@echo off

call "%~dp0\Build.cmd" %* && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 8 "%~dp0\input\day-08\input.txt" && echo.
