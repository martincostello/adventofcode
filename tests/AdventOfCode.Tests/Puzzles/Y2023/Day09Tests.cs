// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 114)]
    [InlineData(true, 2)]
    public void Y2023_Day09_Analyze_Returns_Correct_Value(bool reverse, int expected)
    {
        // Arrange
        string[] histories =
        [
            "0 3 6 9 12 15",
            "1 3 6 10 15 21",
            "10 13 16 21 30 45",
        ];

        // Act
        int actual = Day09.Analyze(histories, reverse);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumNext.ShouldBe(1882395907);
        puzzle.SumPrevious.ShouldBe(1005);
    }
}
