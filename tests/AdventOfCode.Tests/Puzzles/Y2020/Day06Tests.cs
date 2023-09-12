// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 11)]
    [InlineData(true, 6)]
    public void Y2020_Day06_GetSumOfQuestionsWithYesAnswers_Returns_Correct_Value(
        bool byEveryone,
        int expected)
    {
        // Arrange
        string[] values = new[]
        {
            "abc",
            string.Empty,
            "a",
            "b",
            "c",
            string.Empty,
            "ab",
            "ac",
            string.Empty,
            "a",
            "a",
            "a",
            "a",
            string.Empty,
            "b",
        };

        // Act
        int actual = Day06.GetSumOfQuestionsWithYesAnswers(values, byEveryone);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.SumOfQuestionsAnyoneYes.ShouldBe(6542);
        puzzle.SumOfQuestionsEveryoneYes.ShouldBe(3299);
    }
}
