@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 10 1321131112 40 && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 10 1321131112 50 && echo.
