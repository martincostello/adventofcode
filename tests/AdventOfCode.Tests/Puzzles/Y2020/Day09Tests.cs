﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day09_GetWeakNumber_Returns_Correct_Value()
    {
        // Arrange
        long[] values = new long[]
        {
            35,
            20,
            15,
            25,
            47,
            40,
            62,
            55,
            65,
            95,
            102,
            117,
            150,
            182,
            127,
            219,
            299,
            277,
            309,
            576,
        };

        // Act
        long actual = Day09.GetWeakNumber(values, preambleLength: 5);

        // Assert
        actual.ShouldBe(127);
    }

    [Fact]
    public void Y2020_Day09_GetWeakness_Returns_Correct_Value()
    {
        // Arrange
        long[] values = new long[]
        {
            35,
            20,
            15,
            25,
            47,
            40,
            62,
            55,
            65,
            95,
            102,
            117,
            150,
            182,
            127,
            219,
            299,
            277,
            309,
            576,
        };

        // Act
        long actual = Day09.GetWeakness(values, weakNumber: 127);

        // Assert
        actual.ShouldBe(62);
    }

    [Fact]
    public async Task Y2020_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.WeakNumber.ShouldBe(22406676);
        puzzle.Weakness.ShouldBe(2942387);
    }
}
