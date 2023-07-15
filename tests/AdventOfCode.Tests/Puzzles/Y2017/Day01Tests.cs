﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("1122", false, 3)]
    [InlineData("1111", false, 4)]
    [InlineData("1234", false, 0)]
    [InlineData("91212129", false, 9)]
    [InlineData("1212", true, 6)]
    [InlineData("1221", true, 0)]
    [InlineData("123425", true, 4)]
    [InlineData("123123", true, 12)]
    [InlineData("12131415", true, 4)]
    public static void Y2017_Day01_CalculateSum_Returns_Correct_Solution(
        string digits,
        bool nextDigit,
        int expected)
    {
        // Act
        int actual = Day01.CalculateSum(digits, nextDigit);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.CaptchaSolutionNext.ShouldBe(1034);
        puzzle.CaptchaSolutionOpposite.ShouldBe(1356);
    }
}
