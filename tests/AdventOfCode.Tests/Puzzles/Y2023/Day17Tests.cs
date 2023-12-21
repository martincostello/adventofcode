// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day17Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day17_Solve_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "2413432311323",
            "3215453535623",
            "3255245654254",
            "3446585845452",
            "4546657867536",
            "1438598798454",
            "4457876987766",
            "3637877979653",
            "4654967986887",
            "4564679986453",
            "1224686865563",
            "2546548887735",
            "4322674655533",
        ];

        // Act
        int actual = Day17.Solve(values);

        // Assert
        actual.ShouldBe(102);
    }

    [Fact]
    public async Task Y2023_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MinimumHeatLoss.ShouldBe(1013);
    }
}
