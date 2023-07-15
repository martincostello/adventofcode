// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("aA", "")]
    [InlineData("Aa", "")]
    [InlineData("baA", "b")]
    [InlineData("abBA", "")]
    [InlineData("abAB", "abAB")]
    [InlineData("aabAAB", "aabAAB")]
    [InlineData("dabAcCaCBAcCcaDA", "dabCBAcaDA")]
    public static void Y2018_Day05_Reduce_Returns_Correct_Solution(
        string polymer,
        string expected)
    {
        // Act
        string actual = Day05.Reduce(polymer);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("dabAcCaCBAcCcaDA", "daDA")]
    public static void Y2018_Day05_ReduceWithOptimization_Returns_Correct_Solution(
        string polymer,
        string expected)
    {
        // Act
        string actual = Day05.ReduceWithOptimization(polymer);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2018_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.RemainingUnits.ShouldBe(10638);
        puzzle.RemainingUnitsOptimized.ShouldBe(4944);
    }
}
