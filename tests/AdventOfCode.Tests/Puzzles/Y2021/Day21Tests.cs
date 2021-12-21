// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day21Tests : PuzzleTest
{
    public Day21Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day21_Play_Returns_Correct_Value()
    {
        // Arrange
        string[] players =
        {
            "Player 1 starting position: 4",
            "Player 2 starting position: 8",
        };

        // Act
        long actual = Day21.Play(players);

        // Assert
        actual.ShouldBe(739785);
    }

    [Fact]
    public async Task Y2021_Day21_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day21>();

        // Assert
        puzzle.Outcome.ShouldBe(713328);
    }
}
