﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("1", "11")]
    [InlineData("11", "21")]
    [InlineData("21", "1211")]
    [InlineData("1211", "111221")]
    [InlineData("111221", "312211")]
    public static void Y2015_Day10_AsLookAndSay(string value, string expected)
    {
        // Act
        string actual = Day10.AsLookAndSay(value);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day10_Solve_Returns_Correct_Solution()
    {
        // Arrange
        string input = "1321131112";

        // Act
        var puzzle = await SolvePuzzleAsync<Day10>(input);

        // Assert
        puzzle.Solution40.ShouldBe(492982);
        puzzle.Solution50.ShouldBe(6989950);
    }
}
