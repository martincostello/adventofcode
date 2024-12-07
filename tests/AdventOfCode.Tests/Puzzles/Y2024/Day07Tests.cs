// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day07_Calibrate_Returns_Correct_Value()
    {
        // Arrange
        string[] equations =
        [
            "190: 10 19",
            "3267: 81 40 27",
            "83: 17 5",
            "156: 15 6",
            "7290: 6 8 6 15",
            "161011: 16 10 13",
            "192: 17 8 14",
            "21037: 9 7 18 13",
            "292: 11 6 16 20",
        ];

        // Act
        long actual = Day07.Calibrate(equations);

        // Assert
        actual.ShouldBe(3749);
    }

    [Fact]
    public async Task Y2024_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.CalibrationResult.ShouldBe(1153997401072);
    }
}
