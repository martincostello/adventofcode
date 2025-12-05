// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day05_CountFreshIngredients_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "3-5",
            "10-14",
            "16-20",
            "12-18",
            string.Empty,
            "1",
            "5",
            "8",
            "11",
            "17",
            "32",
        ];

        // Act
        int actual = Day05.CountFreshIngredients(values);

        // Assert
        actual.ShouldBe(3);
    }

    [Fact]
    public async Task Y2025_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.FreshIngredientIds.ShouldBe(761);
    }
}
