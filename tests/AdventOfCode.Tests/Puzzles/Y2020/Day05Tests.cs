﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("FBFBBFFRLR", 44, 5, 357)]
    [InlineData("BFFFBBFRRR", 70, 7, 567)]
    [InlineData("FFFBBBFRRR", 14, 7, 119)]
    [InlineData("BBFFBBFRLL", 102, 4, 820)]
    public void Y2020_Day05_Get2020Product_Returns_Correct_Value(
        string boardingPass,
        int expectedRow,
        int expectedColumn,
        int expectedId)
    {
        // Act
        (int actualRow, int actualColumn, int actualId) = Day05.ScanBoardingPass(boardingPass);

        // Assert
        actualRow.ShouldBe(expectedRow);
        actualColumn.ShouldBe(expectedColumn);
        actualId.ShouldBe(expectedId);
    }

    [Fact]
    public async Task Y2020_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.HighestSeatId.ShouldBe(878);
        puzzle.MySeatId.ShouldBe(504);
    }
}
