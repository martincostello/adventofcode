// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

#pragma warning disable SA1010

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("abc", 64, false, 22728)]
    [InlineData("abc", 64, true, 22551, Skip = "Too slow.")]
    public static void Y2016_Day14_GetOneTimePadKeyIndex_Returns_Correct_Solution(
        string salt,
        int ordinal,
        bool useKeyStretching,
        int expected)
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));

        // Act
        int actual = Day14.GetOneTimePadKeyIndex(salt, ordinal, useKeyStretching, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact(Skip = "Too slow.")]
    public async Task Y2016_Day14_Solve_Returns_Correct_Solution()
    {
        // Arrange
        string[] args = ["ihaygndm"];

        // Act
        var puzzle = await SolvePuzzleAsync<Day14>(args);

        // Assert
        puzzle.IndexOfKey64.ShouldBe(15035);
        puzzle.IndexOfKey64WithStretching.ShouldBe(19968);
    }
}
