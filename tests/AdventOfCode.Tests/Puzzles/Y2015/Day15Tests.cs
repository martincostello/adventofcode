// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day15Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2015_Day15_GetHighestTotalCookieScore()
    {
        // Arrange
        string[] collection = new[]
        {
            "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
            "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3",
        };

        // Act
        int actual = Day15.GetHighestTotalCookieScore(collection);

        // Assert
        actual.ShouldBe(62842880);

        // Act
        actual = Day15.GetHighestTotalCookieScore(collection, 500);

        // Assert
        actual.ShouldBe(57600000);
    }

    [Fact]
    public async Task Y2015_Day15_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>();

        // Assert
        puzzle.HighestTotalCookieScore.ShouldBe(222870);
        puzzle.HighestTotalCookieScoreWith500Calories.ShouldBe(117936);
    }
}
