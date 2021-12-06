// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day08Tests : PuzzleTest
{
    public Day08Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public static void Y2018_Day08_ParseTree_Returns_Correct_Solution()
    {
        // Arrange
        int[] tree = { 2, 3, 0, 3, 10, 11, 12, 1, 1, 0, 1, 99, 2, 1, 1, 2 };

        // Act
        long actual = Day08.ParseTree(tree);

        // Assert
        actual.ShouldBe(138);
    }

    [Fact]
    public async Task Y2018_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.SumOfMetadata.ShouldBe(45210);
    }
}
