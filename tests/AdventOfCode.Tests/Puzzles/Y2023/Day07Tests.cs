// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day07_Play_Returns_Correct_Value()
    {
        // Arrange
        string[] hands =
        [
            "32T3K 765",
            "T55J5 684",
            "KK677 28",
            "KTJJT 220",
            "QQQJA 483",
        ];

        // Act
        int actual = Day07.Play(hands);

        // Assert
        actual.ShouldBe(6440);
    }

    [Fact]
    public async Task Y2023_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalWinnings.ShouldBe(249726565);
    }
}
