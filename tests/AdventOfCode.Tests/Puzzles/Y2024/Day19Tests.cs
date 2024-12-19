// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day19Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day19_CountPossibilities_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "r, wr, b, g, bwu, rb, gb, br",
            string.Empty,
            "brwrr",
            "bggr",
            "gbbr",
            "rrbgbr",
            "ubwu",
            "bwurrg",
            "brgr",
            "bbrgwb",
        ];

        // Act
        (int actualPossible, long actualDesigns) = Day19.CountPossibilities(values);

        // Assert
        actualPossible.ShouldBe(6);
        actualDesigns.ShouldBe(16);
    }

    [Fact]
    public async Task Y2024_Day19_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day19>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.PossibleDesigns.ShouldBe(371);
        puzzle.UniqueDesigns.ShouldBe(650354687260341);
    }
}
