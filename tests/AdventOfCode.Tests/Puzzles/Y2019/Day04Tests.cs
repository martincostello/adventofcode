﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

#pragma warning disable SA1010

public sealed class Day04Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("111111", 1, true)]
    [InlineData("111123", 1, true)]
    [InlineData("122345", 1, true)]
    [InlineData("135679", 1, false)]
    [InlineData("123789", 1, false)]
    [InlineData("223450", 1, false)]
    [InlineData("112233", 2, true)]
    [InlineData("123444", 2, false)]
    [InlineData("111122", 2, true)]
    public void Y2019_Day04_Is_Valid_Returns_Correct_Value(string password, int version, bool expected)
    {
        // Act
        bool actual = Day04.IsValid(password, version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2019_Day04_Solve_Returns_Correct_Solution()
    {
        // Arrange
        string[] args = ["138241-674034"];

        // Act
        var puzzle = await SolvePuzzleAsync<Day04>(args);

        // Assert
        puzzle.CountV1.ShouldBe(1890);
        puzzle.CountV2.ShouldBe(1277);
    }
}
