// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class containing tests for the <see cref="Day15"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day15Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day15Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day15Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(new[] { 0, 3, 6 }, 1, 0)]
    [InlineData(new[] { 0, 3, 6 }, 2, 3)]
    [InlineData(new[] { 0, 3, 6 }, 3, 6)]
    [InlineData(new[] { 0, 3, 6 }, 4, 0)]
    [InlineData(new[] { 0, 3, 6 }, 5, 3)]
    [InlineData(new[] { 0, 3, 6 }, 6, 3)]
    [InlineData(new[] { 0, 3, 6 }, 7, 1)]
    [InlineData(new[] { 0, 3, 6 }, 8, 0)]
    [InlineData(new[] { 0, 3, 6 }, 9, 4)]
    [InlineData(new[] { 0, 3, 6 }, 10, 0)]
    [InlineData(new[] { 0, 3, 6 }, 2020, 436)]
    [InlineData(new[] { 1, 3, 2 }, 2020, 1)]
    [InlineData(new[] { 2, 1, 3 }, 2020, 10)]
    [InlineData(new[] { 1, 2, 3 }, 2020, 27)]
    [InlineData(new[] { 2, 3, 1 }, 2020, 78)]
    [InlineData(new[] { 3, 2, 1 }, 2020, 438)]
    [InlineData(new[] { 3, 1, 2 }, 2020, 1836)]
    [InlineData(new[] { 0, 3, 6 }, 30000000, 175594, Skip = "Too slow.")]
    [InlineData(new[] { 1, 3, 2 }, 30000000, 2578, Skip = "Too slow.")]
    [InlineData(new[] { 2, 1, 3 }, 30000000, 3544142, Skip = "Too slow.")]
    [InlineData(new[] { 1, 2, 3 }, 30000000, 261214, Skip = "Too slow.")]
    [InlineData(new[] { 2, 3, 1 }, 30000000, 6895259, Skip = "Too slow.")]
    [InlineData(new[] { 3, 2, 1 }, 30000000, 18, Skip = "Too slow.")]
    [InlineData(new[] { 3, 1, 2 }, 30000000, 362, Skip = "Too slow.")]
    public void Y2020_Day15_GetSpokenNumber_Returns_Correct_Value(
        int[] startingNumbers,
        int number,
        int expected)
    {
        // Act
        int actual = Day15.GetSpokenNumber(startingNumbers, number);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day15_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>("0,5,4,1,10,14,7");

        // Assert
        puzzle.Number2020.ShouldBe(203);
        puzzle.Number30000000.ShouldBe(9007186);
    }
}
