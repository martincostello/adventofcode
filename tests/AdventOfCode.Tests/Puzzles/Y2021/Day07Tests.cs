// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day07Tests : PuzzleTest
{
    public Day07Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day07_AlignSubmarines_Returns_Correct_Value()
    {
        // Arrange
        int[] submarines = { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 };

        // Act
        long actual = Day07.AlignSubmarines(submarines);

        // Assert
        actual.ShouldBe(37);
    }

    [Fact]
    public async Task Y2021_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.FuelConsumed.ShouldBe(335271);
    }
}
