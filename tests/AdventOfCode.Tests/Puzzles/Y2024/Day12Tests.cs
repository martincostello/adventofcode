﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact(Skip = "Not implemented.")]
    public void Y2024_Day12_Solve_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "_",
        ];

        // Act
        int actual = Day12.Solve(values);

        // Assert
        actual.ShouldBe(-1);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2024_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(-1);
    }
}
