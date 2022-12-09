﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day10"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day10Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day10Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day10Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact(Skip = "Not implemented.")]
    public void Y2022_Day10_Solve_Returns_Correct_Value()
    {
        // Arrange
        string[] values = new[]
        {
            "_",
        };

        // Act
        int actual = Day10.Solve(values);

        // Assert
        actual.ShouldBe(-1);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2022_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(-1);
    }
}
