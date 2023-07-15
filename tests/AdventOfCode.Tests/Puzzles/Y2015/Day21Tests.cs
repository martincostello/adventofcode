// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day21Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2015_Day21_Fight()
    {
        var player1 = new Day21.Player(8, 5, 5);
        var player2 = new Day21.Player(12, 7, 2);

        var winner = Day21.Fight(player1, player2);

        // Assert
        winner.ShouldBeSameAs(player1);
    }

    [Fact]
    public async Task Y2015_Day21_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day21>();

        // Assert
        puzzle.MaximumCostToLose.ShouldBe(148);
        puzzle.MinimumCostToWin.ShouldBe(78);
    }
}
