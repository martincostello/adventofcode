@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 12 "%~dp0\..\input\day-12\input.txt" && echo.
