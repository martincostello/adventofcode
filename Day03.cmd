@echo off

call "%~dp0\Build.cmd" %* && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 3 "%~dp0\input\day-03\input.txt" && echo.
