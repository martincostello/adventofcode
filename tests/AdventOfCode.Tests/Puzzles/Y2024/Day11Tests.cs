// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("0 1 10 99 999", 1, 7)]
    [InlineData("125 17", 6, 22)]
    [InlineData("125 17", 25, 55312)]
    public void Y2024_Day11_Blink_Returns_Correct_Value(string stones, int blinks, long expected)
    {
        // Act
        long actual = Day11.Blink(stones, blinks);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.Solution1.ShouldBe(220722);
        puzzle.Solution2.ShouldBe(261952051690787);
    }
}
