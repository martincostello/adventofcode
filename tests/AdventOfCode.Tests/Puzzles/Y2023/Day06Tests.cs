// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day06_Race_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "Time:      7  15   30",
            "Distance:  9  40  200",
        ];

        // Act
        int actual = Day06.Race(values);

        // Assert
        actual.ShouldBe(288);
    }

    [Fact]
    public async Task Y2023_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.CombinationsProduct.ShouldBe(741000);
    }
}
