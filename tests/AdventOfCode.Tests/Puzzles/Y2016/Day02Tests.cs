// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "UDLR" }, "5")]
    [InlineData(new[] { "ULL", "RRDDD", "LURDL", "UUUUD" }, "1985")]
    public static void Y2016_Day02_GetBathroomCode_Returns_Correct_Solution_Digits(string[] instructions, string expected)
    {
        // Act
        string actual = Day02.GetBathroomCode(instructions, Day02.DigitGrid);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(new[] { "UDLR" }, "6")]
    [InlineData(new[] { "ULL", "RRDDD", "LURDL", "UUUUD" }, "5DB3")]
    public static void Y2016_Day02_GetBathroomCode_Returns_Correct_Solution_Alphanumeric(string[] instructions, string expected)
    {
        // Act
        string actual = Day02.GetBathroomCode(instructions, Day02.AlphanumericGrid);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.BathroomCodeDigitKeypad.ShouldBe("14894");
        puzzle.BathroomCodeAlphanumericKeypad.ShouldBe("26B96");
    }
}
