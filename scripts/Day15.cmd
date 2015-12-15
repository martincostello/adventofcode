@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 15 "%~dp0\..\input\day-15\input.txt" && echo.
