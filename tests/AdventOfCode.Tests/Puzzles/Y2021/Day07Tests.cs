// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day07Tests : PuzzleTest
{
    public Day07Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(false, 37)]
    [InlineData(true, 168)]
    public void Y2021_Day07_AlignSubmarines_Returns_Correct_Value(bool withVariableBurnRate, long expected)
    {
        // Arrange
        int[] submarines = { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 };

        // Act
        long actual = Day07.AlignSubmarines(submarines, withVariableBurnRate);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.FuelConsumedWithConstantBurnRate.ShouldBe(335271);
        puzzle.FuelConsumedWithVariableBurnRate.ShouldBe(95851339);
    }
}
