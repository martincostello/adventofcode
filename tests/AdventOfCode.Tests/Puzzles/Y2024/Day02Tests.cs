// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 2)]
    [InlineData(true, 4)]
    public void Y2024_Day02_CountSafeReports_Returns_Correct_Value(
        bool useProblemDampener,
        int expected)
    {
        // Arrange
        string[] values =
        [
            "7 6 4 2 1",
            "1 2 7 8 9",
            "9 7 6 2 1",
            "1 3 2 4 5",
            "8 6 4 4 1",
            "1 3 6 7 9",
        ];

        // Act
        int actual = Day02.CountSafeReports(values, useProblemDampener);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SafeReports.ShouldBe(236);
        puzzle.SafeReportsWithDampener.ShouldBe(308);
    }
}
