// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day17Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(0, 3, 5)]
    [InlineData(1, 3, 11)]
    [InlineData(2, 3, 21)]
    [InlineData(3, 3, 38)]
    [InlineData(6, 3, 112)]
    [InlineData(6, 4, 848)]
    public void Y2020_Day17_GetActiveCubes_Returns_Correct_Value(
        int cycles,
        int dimensions,
        int expected)
    {
        // Arrange
        string[] layout =
        [
            ".#.",
            "..#",
            "###",
        ];

        // Act
        (int actual, _) = Day17.GetActiveCubes(layout, cycles, dimensions, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.ActiveCubes3D.ShouldBe(388);
        puzzle.ActiveCubes4D.ShouldBe(2280);
    }
}
