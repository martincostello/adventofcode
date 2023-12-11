// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(1, 374)]
    [InlineData(10, 1030)]
    [InlineData(100, 8410)]
    public void Y2023_Day11_Analyze_Returns_Correct_Value(int expansion, long expected)
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
        long actual = Day11.Analyze(image, expansion);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfLengthsSmall.ShouldBe(9605127);
        puzzle.SumOfLengthsLarge.ShouldBe(-1);
    }
}
