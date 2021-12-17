// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day17Tests : PuzzleTest
{
    public Day17Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day17_Calculate_Returns_Correct_Sum()
    {
        // Arrange
        string target = "target area: x=20..30, y=-10..-5";

        // Act
        int actual = Day17.Calculate(target);

        // Assert
        actual.ShouldBe(45);
    }

    [Fact]
    public async Task Y2021_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.Apogee.ShouldBe(8646);
    }
}
