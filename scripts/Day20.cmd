@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 20 34000000 && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 20 34000000 50 && echo.
