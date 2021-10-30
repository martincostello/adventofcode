// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using static MartinCostello.AdventOfCode.Benchmarks.PuzzleBenchmarks;

namespace MartinCostello.AdventOfCode.Benchmarks;

public static class BenchmarkTests
{
    public static IEnumerable<object[]> Benchmarks()
    {
        foreach (object puzzle in PuzzleBenchmarks.Puzzles())
        {
            yield return new object[] { puzzle };
        }
    }

    [Theory]
    [MemberData(nameof(Benchmarks))]
    public static async Task Can_Run_Benchmarks(PuzzleInput input)
    {
        // Arrange
        IPuzzle puzzle = input.Puzzle;
        string[] args = input.Args;

        // Act (no Assert)
        await puzzle.SolveAsync(args, CancellationToken.None);
    }
}
