@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 19 "%~dp0\..\input\day-19\input.txt" calibrate && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 19 "%~dp0\..\input\day-19\input.txt" fabricate && echo.
