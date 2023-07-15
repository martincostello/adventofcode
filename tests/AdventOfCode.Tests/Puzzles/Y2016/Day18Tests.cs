// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day18Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("..^^.", 3, 6)]
    [InlineData(".^^.^.^^^^", 10, 38)]
    public void Y2016_Day18_FindSafeTileCount_Returns_Correct_Solution(string firstRowTiles, int rows, int expected)
    {
        // Act
        (int actual, _) = Day18.FindSafeTileCount(firstRowTiles, rows, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.SafeTileCount40.ShouldBe(1987);
        puzzle.SafeTileCount400000.ShouldBe(19984714);
    }
}
