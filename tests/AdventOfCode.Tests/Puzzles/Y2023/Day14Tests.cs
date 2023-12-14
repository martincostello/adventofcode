// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day14_ComputeLoad_Returns_Correct_Value()
    {
        // Arrange
        string[] rocks =
        [
            "O....#....",
            "O.OO#....#",
            ".....##...",
            "OO.#O....O",
            ".O.....O#.",
            "O.#..O.#.#",
            "..O..#O..O",
            ".......O..",
            "#....###..",
            "#OO..#....",
        ];

        // Act
        int actual = Day14.ComputeLoad(rocks);

        // Assert
        actual.ShouldBe(136);
    }

    [Fact]
    public async Task Y2023_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalLoad.ShouldBe(108641);
    }
}
