// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day04Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day04_Score_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
            "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
            "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
            "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
            "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
            "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
        ];

        // Act
        (int actualPoints, int actualCount) = Day04.Score(values);

        // Assert
        actualPoints.ShouldBe(13);
        actualCount.ShouldBe(30);
    }

    [Fact]
    public async Task Y2023_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalPoints.ShouldBe(21138);
        puzzle.TotalScratchcards.ShouldBe(7185540);
    }
}
