// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 }, 35)]
    [InlineData(new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3 }, 220)]
    public void Y2020_Day10_GetJoltageProduct_Returns_Correct_Value(int[] joltages, int expected)
    {
        // Act
        int actual = Day10.GetJoltageProduct(joltages);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 }, 8)]
    [InlineData(new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3 }, 19208)]
    public void Y2020_Day10_GetValidArrangements_Returns_Correct_Value(int[] joltages, int expected)
    {
        // Act
        long actual = Day10.GetValidArrangements(joltages);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.JoltageProduct.ShouldBe(2775);
        puzzle.ValidArrangements.ShouldBe(518344341716992L);
    }
}
