// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day04Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2021_Day04_PlayBingo_Returns_Correct_Value()
    {
        // Arrange
        string[] game =
        [
            "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
            string.Empty,
            "22 13 17 11  0",
            " 8  2 23  4 24",
            "21  9 14 16  7",
            " 6 10  3 18  5",
            " 1 12 20 15 19",
            string.Empty,
            " 3 15  0  2 22",
            " 9 18 13 17  5",
            "19  8  7 25 23",
            "20 11 10 24  4",
            "14 21 16 12  6",
            string.Empty,
            "14 21 17 24  4",
            "10 16 15  9 19",
            "18  8 23 26 20",
            "22 11 13  6  5",
            " 2  0 12  3  7",
        ];

        // Act
        (int firstWinningScore, int lastWinningScore) = Day04.PlayBingo(game);

        // Assert
        firstWinningScore.ShouldBe(4512);
        lastWinningScore.ShouldBe(1924);
    }

    [Fact]
    public async Task Y2021_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.FirstWinningScore.ShouldBe(41668);
        puzzle.LastWinningScore.ShouldBe(10478);
    }
}
