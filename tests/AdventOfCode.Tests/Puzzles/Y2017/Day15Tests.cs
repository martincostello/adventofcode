// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

#pragma warning disable SA1010

public sealed class Day15Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(1, 588)]
    [InlineData(2, 309)]
    public static void Y2017_Day15_GetMatchingPairs_Returns_Correct_Solution(int version, int expected)
    {
        // Arrange
        string[] seeds = ["Generator A starts with 65", "Generator B starts with 8921"];

        // Act
        int actual = Day15.GetMatchingPairs(seeds, version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>();

        // Assert
        puzzle.FinalCountV1.ShouldBe(594);
        puzzle.FinalCountV2.ShouldBe(328);
    }
}
