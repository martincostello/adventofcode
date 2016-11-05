#!/bin/sh
dotnet restore --verbosity minimal
dotnet build src/AdventOfCode
dotnet test tests/AdventOfCode.Tests
