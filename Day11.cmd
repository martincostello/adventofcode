@echo off

call "%~dp0\Build.cmd" %* && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 11 cqjxjnds && echo.
"%~dp0\src\BuildOutput\AdventOfCode2015.exe" 11 cqjxxyzz && echo.
