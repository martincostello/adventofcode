@echo off

call "%~dp0\..\Build.cmd" %* && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 4 iwrupvqb 5 && echo.
"%~dp0\..\src\BuildOutput\AdventOfCode2015.exe" 4 iwrupvqb 6 && echo.
