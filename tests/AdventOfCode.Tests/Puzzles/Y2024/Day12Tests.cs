// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static TheoryData<string[], bool, int> Examples()
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
                false,
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
                false,
                1930
            },
            {
                new[]
                {
                    "AAAA",
                    "BBCD",
                    "BBCC",
                    "EEEC",
                },
                true,
                80
            },
            {
                new[]
                {
                    "EEEEE",
                    "EXXXX",
                    "EEEEE",
                    "EXXXX",
                    "EEEEE",
                },
                true,
                236
            },
            {
                new[]
                {
                    "AAAAAA",
                    "AAABBA",
                    "AAABBA",
                    "ABBAAA",
                    "ABBAAA",
                    "AAAAAA",
                },
                true,
                368
            },
        };
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void Y2024_Day12_Compute_Returns_Correct_Value(string[] values, bool bulkDiscount, int expected)
    {
        // Arrange
        using var cts = Timeout();

        // Act
        int actual = Day12.Compute(values, bulkDiscount, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalPriceWithoutDiscount.ShouldBe(1465112);
        puzzle.TotalPriceWithDiscount.ShouldBe(-1);
    }
}
