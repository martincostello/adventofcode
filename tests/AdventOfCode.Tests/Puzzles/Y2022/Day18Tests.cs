﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day18Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "1,1,1" }, false, 6)]
    [InlineData(new[] { "1,1,1" }, true, 6)]
    [InlineData(new[] { "1,1,1", "2,1,1" }, false, 10)]
    [InlineData(new[] { "1,1,1", "2,1,1" }, true, 10)]
    [InlineData(new[] { "1,1,1", "2,1,1", "1,2,1", "2,2,1", "1,1,2", "2,1,2", "1,2,2", "2,2,2" }, false, 4 * 6)]
    [InlineData(new[] { "1,1,1", "2,1,1", "1,2,1", "2,2,1", "1,1,2", "2,1,2", "1,2,2", "2,2,2" }, true, 4 * 6)]
    [InlineData(new[] { "2,2,2", "1,2,2", "3,2,2", "2,1,2", "2,3,2", "2,2,1", "2,2,3", "2,2,4", "2,2,6", "1,2,5", "3,2,5", "2,1,5", "2,3,5" }, false, 64)]
    [InlineData(new[] { "2,2,2", "1,2,2", "3,2,2", "2,1,2", "2,3,2", "2,2,1", "2,2,3", "2,2,4", "2,2,6", "1,2,5", "3,2,5", "2,1,5", "2,3,5" }, true, 58)]
    [InlineData(new[] { "0,0,0", "0,1,0", "0,2,0", "1,0,0", "1,1,0", "1,2,0", "2,0,0", "2,1,0", "2,2,0", "0,0,1", "0,1,1", "0,2,1", "1,0,1", "1,2,1", "2,0,1", "2,1,1", "2,2,1", "0,0,2", "0,1,2", "0,2,2", "1,0,2", "1,1,2", "1,2,2", "2,0,2", "2,1,2", "2,2,2" }, false, (9 * 6) + 6)]
    [InlineData(new[] { "0,0,0", "0,1,0", "0,2,0", "1,0,0", "1,1,0", "1,2,0", "2,0,0", "2,1,0", "2,2,0", "0,0,1", "0,1,1", "0,2,1", "1,0,1", "1,2,1", "2,0,1", "2,1,1", "2,2,1", "0,0,2", "0,1,2", "0,2,2", "1,0,2", "1,1,2", "1,2,2", "2,0,2", "2,1,2", "2,2,2" }, true, 9 * 6)]
    [InlineData(new[] { "0,0,0", "0,1,0", "0,2,0", "1,0,0", "1,1,0", "1,2,0", "2,0,0", "2,1,0", "2,2,0", "0,0,1", "0,1,1", "0,2,1", "1,0,1", "1,1,1", "1,2,1", "2,0,1", "2,1,1", "2,2,1", "0,0,2", "0,1,2", "0,2,2", "1,0,2", "1,1,2", "1,2,2", "2,0,2", "2,1,2", "2,2,2" }, false, 9 * 6)]
    [InlineData(new[] { "0,0,0", "0,1,0", "0,2,0", "1,0,0", "1,1,0", "1,2,0", "2,0,0", "2,1,0", "2,2,0", "0,0,1", "0,1,1", "0,2,1", "1,0,1", "1,1,1", "1,2,1", "2,0,1", "2,1,1", "2,2,1", "0,0,2", "0,1,2", "0,2,2", "1,0,2", "1,1,2", "1,2,2", "2,0,2", "2,1,2", "2,2,2" }, true, 9 * 6)]
    public void Y2022_Day18_GetSurfaceArea_Returns_Correct_Value(string[] cubes, bool excludeInterior, int expected)
    {
        // Act
        int actual = Day18.GetSurfaceArea(cubes, excludeInterior, TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalDropletSurfaceArea.ShouldBe(4340);
        puzzle.ExternalDropletSurfaceArea.ShouldBe(2468);
    }
}
