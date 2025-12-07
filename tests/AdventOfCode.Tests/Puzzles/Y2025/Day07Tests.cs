// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day07_Simulate_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            ".......S.......",
            "...............",
            ".......^.......",
            "...............",
            "......^.^......",
            "...............",
            ".....^.^.^.....",
            "...............",
            "....^.^...^....",
            "...............",
            "...^.^...^.^...",
            "...............",
            "..^...^.....^..",
            "...............",
            ".^.^.^.^.^...^.",
            "...............",
        ];

        // Act
        (int actualSplits, long actualTimelines) = Day07.Simulate(values, TestContext.Current.CancellationToken);

        // Assert
        actualSplits.ShouldBe(21);
        actualTimelines.ShouldBe(40);
    }

    [Fact]
    public async Task Y2025_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution1.ShouldBe(1566);
        puzzle.Solution2.ShouldBe(Puzzle.Unsolved);
    }
}
