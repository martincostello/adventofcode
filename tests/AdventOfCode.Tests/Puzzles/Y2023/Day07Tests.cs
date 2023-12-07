// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 6440)]
    [InlineData(true, 5905)]
    public void Y2023_Day07_Play_Returns_Correct_Value(bool useJokers, int expected)
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
        int actual = Day07.Play(hands, useJokers);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalWinnings.ShouldBe(249726565);
        puzzle.TotalWinningsWithJokers.ShouldBeGreaterThan(250096106);
        puzzle.TotalWinningsWithJokers.ShouldBeLessThan(255202570);
        puzzle.TotalWinningsWithJokers.ShouldNotBeOneOf([251844931, 252032536, 251323565, 251376959, 251378963]);
        puzzle.TotalWinningsWithJokers.ShouldBe(-1);
    }
}
