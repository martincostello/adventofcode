@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 13 "%~dp0\..\input\day-13\input.txt" && echo.
