// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 7)]
    [InlineData(true, 5)]
    public void Y2021_Day01_GetDepthMeasurementIncreases_Returns_Correct_Value(
        bool useSlidingWindow,
        int expected)
    {
        // Arrange
        int[] depthMeasurements = { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };

        // Act
        int actual = Day01.GetDepthMeasurementIncreases(depthMeasurements, useSlidingWindow);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.DepthIncreases.ShouldBe(1532);
        puzzle.DepthIncreasesWithSlidingWindow.ShouldBe(1571);
    }
}
