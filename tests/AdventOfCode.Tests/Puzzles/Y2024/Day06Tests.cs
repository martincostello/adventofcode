// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day06_Patrol_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "....#.....",
            ".........#",
            "..........",
            "..#.......",
            ".......#..",
            "..........",
            ".#..^.....",
            "........#.",
            "#.........",
            "......#...",
        ];

        using var cts = Timeout();

        // Act
        (int actualPositions, int actualLoops) = Day06.Patrol(values, cts.Token);

        // Assert
        actualPositions.ShouldBe(41);
        actualLoops.ShouldBe(6);
    }

    [Fact]
    public async Task Y2024_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.Solution1.ShouldBe(5453);
        puzzle.Solution2.ShouldBe(2188);
    }
}
