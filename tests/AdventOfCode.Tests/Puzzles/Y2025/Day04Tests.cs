// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day04Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day04_FindAccessible_Returns_Correct_Total()
    {
        // Arrange
        string[] diagram =
        [
            "..@@.@@@@.",
            "@@@.@.@.@@",
            "@@@@@.@.@@",
            "@.@@@@..@.",
            "@@.@@@@.@@",
            ".@@@@@@@.@",
            ".@.@.@.@@@",
            "@.@@@.@@@@",
            ".@@@@@@@@.",
            "@.@.@@@.@.",
        ];

        // Act
        long actual = Day04.FindAccessible(diagram);

        // Assert
        actual.ShouldBe(13);
    }

    [Fact]
    public async Task Y2025_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.AccessibleRolls.ShouldBe(1587);
    }
}
