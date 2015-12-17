@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 17 "%~dp0\..\input\day-17\input.txt" 150 && echo.
