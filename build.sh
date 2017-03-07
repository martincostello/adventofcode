#!/bin/sh
export artifacts=$(dirname "$(readlink -f "$0")")/artifacts
export configuration=Release

dotnet restore AdventOfCode.sln --verbosity minimal || exit 1
dotnet build AdventOfCode.sln --output $artifacts --configuration $configuration || exit 1
dotnet test tests/AdventOfCode.Tests/AdventOfCode.Tests.csproj --output $artifacts --configuration $configuration || exit 1
