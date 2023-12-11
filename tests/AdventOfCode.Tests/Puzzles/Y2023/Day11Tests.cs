// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day11_Analyze_Returns_Correct_Value()
    {
        // Arrange
        string[] image =
        [
            "...#......",
            ".......#..",
            "#.........",
            "..........",
            "......#...",
            ".#........",
            ".........#",
            "..........",
            ".......#..",
            "#...#.....",
        ];

        // Act
        int actual = Day11.Analyze(image);

        // Assert
        actual.ShouldBe(374);
    }

    [Fact]
    public async Task Y2023_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfLengths.ShouldBe(9605127);
    }
}
