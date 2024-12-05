// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 143)]
    [InlineData(true, 123)]
    public void Y2024_Day05_Sort_Returns_Correct_Value(bool fix, int expected)
    {
        // Arrange
        string[] rules =
        [
            "47|53",
            "97|13",
            "97|61",
            "97|47",
            "75|29",
            "61|13",
            "75|53",
            "29|13",
            "97|29",
            "53|29",
            "61|53",
            "97|53",
            "61|29",
            "47|13",
            "75|47",
            "97|75",
            "47|61",
            "75|61",
            "47|29",
            "75|13",
            "53|13",
            string.Empty,
            "75,47,61,53,29",
            "97,61,53,29,13",
            "75,29,13",
            "75,97,47,61,53",
            "61,13,29",
            "97,13,75,29,47",
        ];

        // Act
        int actual = Day05.Sort(rules, fix);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MiddlePageSumCorrect.ShouldBe(5948);
        puzzle.MiddlePageSumIncorrect.ShouldBe(3062);
    }
}
