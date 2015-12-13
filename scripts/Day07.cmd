@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 7 "%~dp0\..\input\day-07\input.txt" && echo.
