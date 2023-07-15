// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2017_Day14_GetSquaresUsed_Returns_Correct_Value()
    {
        // Arrange
        string key = "flqrgnkx";

        // Act
        int actual = Day14.GetSquaresUsed(key);

        // Assert
        actual.ShouldBe(8108);
    }

    [Fact]
    public async Task Y2017_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>("hwlqcszp");

        // Assert
        puzzle.SquaresUsed.ShouldBe(8304);
    }
}
