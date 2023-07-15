// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day17Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2021_Day17_Calculate_Returns_Correct_Value()
    {
        // Arrange
        string target = "target area: x=20..30, y=-10..-5";

        // Act
        (int actualMaxApogee, int actualCount) = Day17.Calculate(target);

        // Assert
        actualMaxApogee.ShouldBe(45);
        actualCount.ShouldBe(112);
    }

    [Fact]
    public async Task Y2021_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.MaxApogee.ShouldBe(8646);
        puzzle.Count.ShouldBe(5945);
    }
}
