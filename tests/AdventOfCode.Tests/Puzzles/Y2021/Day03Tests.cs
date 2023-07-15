// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2021_Day03_GetPowerConsumption_Returns_Correct_Value()
    {
        // Arrange
        string[] diagnosticReport =
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        // Act
        int actual = Day03.GetPowerConsumption(diagnosticReport);

        // Assert
        actual.ShouldBe(198);
    }

    [Fact]
    public void Y2021_Day03_GetLifeSupportRating_Returns_Correct_Value()
    {
        // Arrange
        string[] diagnosticReport =
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        // Act
        int actual = Day03.GetLifeSupportRating(diagnosticReport);

        // Assert
        actual.ShouldBe(230);
    }

    [Fact]
    public async Task Y2021_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.PowerConsumption.ShouldBe(3633500);
        puzzle.LifeSupportRating.ShouldBe(4550283);
    }
}
