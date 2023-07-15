// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("abcdefgh", "abcdffaa")]
    [InlineData("ghijklmn", "ghjaabcc")]
    public static void Y2015_Day11_GenerateNextPassword(string current, string expected)
    {
        // Act
        string actual = Day11.GenerateNextPassword(current);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("hijklmmn", false)]
    [InlineData("abbceffg", true)]
    [InlineData("abbcegjk", false)]
    public static void Y2015_Day11_HasMoreThanOneDistinctPairOfLetters(string value, bool expected)
    {
        // Act
        bool actual = Day11.HasMoreThanOneDistinctPairOfLetters(value.ToCharArray());

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day11_Solve_Returns_Correct_Solution()
    {
        // Arrange
        string input = "cqjxjnds";

        // Act
        var puzzle = await SolvePuzzleAsync<Day11>(input);

        // Assert
        puzzle.FirstPassword.ShouldBe("cqjxxyzz");
        puzzle.SecondPassword.ShouldBe("cqkaabcc");
    }
}
