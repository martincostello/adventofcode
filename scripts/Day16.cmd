@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 16 "%~dp0\..\input\day-16\input.txt" && echo.
