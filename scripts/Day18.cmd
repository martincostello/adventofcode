@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 18 "%~dp0\..\input\day-18\input.txt" 100 && echo.
