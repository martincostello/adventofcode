@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 25 2947 3029 && echo.
