// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using static MartinCostello.AdventOfCode.Benchmarks.PuzzleBenchmarks;

namespace MartinCostello.AdventOfCode.Benchmarks;

public static class BenchmarkTests
{
    public static TheoryData<PuzzleInput> Benchmarks()
    {
        var result = new TheoryData<PuzzleInput>();

        foreach (var puzzle in PuzzleBenchmarks.Puzzles().Cast<PuzzleInput>())
        {
            result.Add(puzzle);
        }

        return result;
    }

    [Theory]
    [MemberData(nameof(Benchmarks))]
    public static async Task Can_Run_Benchmarks(PuzzleInput input)
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));

        // Act and Assert
        await Should.NotThrowAsync(async () => await input.SolveAsync(cts.Token));
    }
}
