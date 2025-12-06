// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 14)]
    [InlineData(true, 34)]
    public void Y2024_Day08_FindAntinodes_Returns_Correct_Value(bool resonantHarmonics, int expected)
    {
        // Arrange
        string[] values =
        [
            "............",
            "........0...",
            ".....0......",
            ".......0....",
            "....0.......",
            "......A.....",
            "............",
            "............",
            "........A...",
            ".........A..",
            "............",
            "............",
        ];

        // Act
        int actual = Day08.FindAntinodes(values, resonantHarmonics);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.Solution1.ShouldBe(308);
        puzzle.Solution2.ShouldBe(1147);
    }
}
