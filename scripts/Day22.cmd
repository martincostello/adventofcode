@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 22 && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 22 hard && echo.
