// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
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
        (int sumOfRiskLevels, int areaOfThreeLargestBasins) = Day09.AnalyzeRisk(heightmap);

        // Assert
        sumOfRiskLevels.ShouldBe(15);
        areaOfThreeLargestBasins.ShouldBe(1134);
    }

    [Fact]
    public async Task Y2021_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.SumOfRiskLevels.ShouldBe(607);
        puzzle.AreaOfThreeLargestBasins.ShouldBe(900864);
    }
}
