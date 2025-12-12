// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day12_Arrange_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "0:",
            "###",
            "##.",
            "##.",
            string.Empty,
            "1:",
            "###",
            "##.",
            ".##",
            string.Empty,
            "2:",
            ".##",
            "###",
            "##.",
            string.Empty,
            "3:",
            "##.",
            "###",
            "##.",
            string.Empty,
            "4:",
            "###",
            "#..",
            "###",
            string.Empty,
            "5:",
            "###",
            ".#.",
            "###",
            string.Empty,
            "4x4: 0 0 0 0 2 0",
            "12x5: 1 0 1 0 2 2",
            "12x5: 1 0 1 0 3 2",
        ];

        // Act
        int actual = Day12.Arrange(values, TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldBe(2);
    }

    [Fact]
    public async Task Y2025_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(Puzzle.Unsolved);
    }
}
