﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("1-3 a: abcde", 1, true)]
    [InlineData("1-3 b: cdefg", 1, false)]
    [InlineData("2-9 c: ccccccccc", 1, true)]
    [InlineData("1-3 a: abcde", 2, true)]
    [InlineData("1-3 b: cdefg", 2, false)]
    [InlineData("2-9 c: ccccccccc", 2, false)]
    public void Y2020_Day02_IsPasswordValid_Returns_Correct_Value(string value, int policyVersion, bool expected)
    {
        // Act
        bool actual = Day02.IsPasswordValid(value, policyVersion);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    public void Y2020_Day02_GetValidPasswordCount_Returns_Correct_Value(int policyVersion, int expected)
    {
        // Arrange
        string[] values = new[]
        {
            "1-3 a: abcde",
            "1-3 b: cdefg",
            "2-9 c: ccccccccc",
        };

        // Act
        int actual = Day02.GetValidPasswordCount(values, policyVersion);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ValidPasswordsV1.ShouldBe(542);
        puzzle.ValidPasswordsV2.ShouldBe(360);
    }
}
