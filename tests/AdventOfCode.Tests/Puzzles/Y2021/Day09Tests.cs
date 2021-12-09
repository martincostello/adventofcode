// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day09Tests : PuzzleTest
{
    public Day09Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day09_AnalyzeRisk_Returns_Correct_Value()
    {
        // Arrange
        string[] heightmap =
        {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678",
        };

        // Act
        int actual = Day09.AnalyzeRisk(heightmap);

        // Assert
        actual.ShouldBe(15);
    }

    [Fact]
    public async Task Y2021_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.SumOfRiskLevels.ShouldBe(607);
    }
}
