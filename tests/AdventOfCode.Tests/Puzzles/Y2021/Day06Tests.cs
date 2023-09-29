﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

#pragma warning disable SA1010

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(80, 5934)]
    [InlineData(256, 26984457539)]
    public void Y2021_Day06_CountFish_Returns_Correct_Value(int days, long expected)
    {
        // Arrange
        int[] fish = [3, 4, 3, 1, 2];

        // Act
        long actual = Day06.CountFish(fish, days);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.FishCount80.ShouldBe(377263);
        puzzle.FishCount256.ShouldBe(1695929023803);
    }
}
