// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day03_Returns_Correct_Solution_For_Path()
    {
        // Arrange
        string[] grid =
        [
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#",
        ];

        // Act
        int actual = Day03.GetTreeCollisionCount(grid, 3, 1);

        // Assert
        actual.ShouldBe(7);
    }

    [Fact]
    public async Task Y2020_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.TreeCollisions.ShouldBe(216);
        puzzle.ProductOfTreeCollisions.ShouldBe(6708199680L);
    }
}
