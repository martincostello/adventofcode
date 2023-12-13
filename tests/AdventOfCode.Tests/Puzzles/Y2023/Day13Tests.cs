// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day13Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 405)]
    [InlineData(true, 400)]
    public void Y2023_Day13_Summarize_Returns_Correct_Value(bool fixSmudges, int expected)
    {
        // Arrange
        string[] notes =
        [
            "#.##..##.",
            "..#.##.#.",
            "##......#",
            "##......#",
            "..#.##.#.",
            "..##..##.",
            "#.#.##.#.",
            string.Empty,
            "#...##..#",
            "#....#..#",
            "..##..###",
            "#####.##.",
            "#####.##.",
            "..##..###",
            "#....#..#",
        ];

        // Act
        int actual = Day13.Summarize(notes, fixSmudges);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day13_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day13>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Summary.ShouldBe(35538);
        puzzle.SummaryWithSmudgesCleaned.ShouldBeGreaterThan(21020);
        puzzle.SummaryWithSmudgesCleaned.ShouldBeLessThan(33348);
        puzzle.SummaryWithSmudgesCleaned.ShouldBe(-1);
    }
}
