﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day15Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 40)]
    [InlineData(true, 315)]
    public void Y2021_Day15_GetRiskLevel_Returns_Correct_Value(bool largeMap, int expected)
    {
        // Arrange
        string[] riskMap =
        [
            "1163751742",
            "1381373672",
            "2136511328",
            "3694931569",
            "7463417111",
            "1319128137",
            "1359912421",
            "3125421639",
            "1293138521",
            "2311944581",
        ];

        // Act
        int actual = Day15.GetRiskLevel(riskMap, largeMap);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day15_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>();

        // Assert
        puzzle.RiskLevelSmall.ShouldBe(487);
        puzzle.RiskLevelLarge.ShouldBe(2821);
    }
}
