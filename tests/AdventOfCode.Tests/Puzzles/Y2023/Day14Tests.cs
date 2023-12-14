// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(0, 136)]
    [InlineData(1, 87)]
    [InlineData(2, 69)]
    [InlineData(3, 69)]
    [InlineData(1000000000, 64)]
    public void Y2023_Day14_ComputeLoad_Returns_Correct_Value(int rotations, int expected)
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

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
        (int actual, _) = Day14.ComputeLoad(rocks, rotations, Logger, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalLoad.ShouldBe(108641);
        puzzle.TotalLoadWithSpinCycle.ShouldBe(84328);
    }
}
