// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day01_SumCalibrations_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "1abc2",
            "pqr3stu8vwx",
            "a1b2c3d4e5f",
            "treb7uchet",
        ];

        // Act
        int actual = Day01.SumCalibrations(values);

        // Assert
        actual.ShouldBe(142);
    }

    [Fact]
    public async Task Y2023_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfCalibrations.ShouldBe(54697);
    }
}
