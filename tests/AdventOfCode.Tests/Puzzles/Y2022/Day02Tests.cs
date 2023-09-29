// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 15)]
    [InlineData(true, 12)]
    public void Y2022_Day02_GetTotalScore_Returns_Correct_Values(bool containsDesiredOutcome, int expected)
    {
        // Arrange
        string[] moves =
        [
            "A Y",
            "B X",
            "C Z",
        ];

        // Act
        int actual = Day02.GetTotalScore(moves, containsDesiredOutcome);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalScoreForMoves.ShouldBe(13675);
        puzzle.TotalScoreForOutcomes.ShouldBe(14184);
    }
}
