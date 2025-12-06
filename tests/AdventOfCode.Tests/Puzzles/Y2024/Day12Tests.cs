// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static TheoryData<string[], int> Examples()
    {
        return new()
        {
            {
                new[]
                {
                    "OOOOO",
                    "OXOXO",
                    "OOOOO",
                    "OXOXO",
                    "OOOOO",
                },
                772
            },
            {
                new[]
                {
                    "RRRRIICCFF",
                    "RRRRIICCCF",
                    "VVRRRCCFFF",
                    "VVRCCCJFFF",
                    "VVVVCJJCFE",
                    "VVIVCCJJEE",
                    "VVIIICJJEE",
                    "MIIIIIJJEE",
                    "MIIISIJEEE",
                    "MMMISSJEEE",
                },
                1930
            },
        };
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void Y2024_Day12_Compute_Returns_Correct_Value(string[] values, int expected)
    {
        // Arrange
        using var cts = Timeout();

        // Act
        int actual = Day12.Compute(values, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.Solution.ShouldBe(1465112);
    }
}
