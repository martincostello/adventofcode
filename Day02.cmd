@echo off

call "%~dp0\Build.cmd" %* && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 2 "%~dp0\input\day-02\input.txt" && echo.
