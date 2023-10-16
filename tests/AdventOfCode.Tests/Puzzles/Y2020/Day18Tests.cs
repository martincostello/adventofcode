// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day18Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(1, "0", 0)]
    [InlineData(1, "1", 1)]
    [InlineData(1, "11", 11)]
    [InlineData(1, "5 * 3", 15)]
    [InlineData(1, "(5 * 3)", 15)]
    [InlineData(1, "((5 * 3))", 15)]
    [InlineData(1, "11 + 3", 14)]
    [InlineData(1, "(11 + 3)", 14)]
    [InlineData(1, "((11 + 3))", 14)]
    [InlineData(1, "1 + 2 * 3 + 4 * 5 + 6", 71)]
    [InlineData(1, "1 + (2 * 3) + (4 * (5 + 6))", 51)]
    [InlineData(1, "2 * 3 + (4 * 5)", 26)]
    [InlineData(1, "5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
    [InlineData(1, "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
    [InlineData(1, "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
    [InlineData(2, "1 + 2 * 3 + 4 * 5 + 6", 231)]
    [InlineData(2, "1 + (2 * 3) + (4 * (5 + 6))", 51)]
    [InlineData(2, "2 * 3 + (4 * 5)", 46)]
    [InlineData(2, "5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
    [InlineData(2, "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
    [InlineData(2, "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
    public void Y2020_Day18_Evaluate_Returns_Correct_Value(
        int version,
        string expression,
        long expected)
    {
        // Act
        long actual = Day18.Evaluate([expression], version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.SumV1.ShouldBe(2743012121210L);
        puzzle.SumV2.ShouldBe(65658760783597L);
    }
}
