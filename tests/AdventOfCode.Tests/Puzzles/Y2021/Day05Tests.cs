// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 5)]
    [InlineData(true, 12)]
    public void Y2021_Day05_NavigateVents_Returns_Correct_Value(bool useDiagonals, int expected)
    {
        // Arrange
        string[] game =
        {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2",
        };

        // Act
        int actual = Day05.NavigateVents(game, useDiagonals);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.OverlappingVents.ShouldBe(5690);
        puzzle.OverlappingVentsWithDiagonals.ShouldBe(17741);
    }
}
