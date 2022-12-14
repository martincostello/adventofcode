﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day14"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day14Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day14Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day14Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day14_Simulate_Returns_Correct_Value()
    {
        // Arrange
        string[] paths = new[]
        {
            "498,4 -> 498,6 -> 496,6",
            "503,4 -> 502,4 -> 502,9 -> 494,9",
        };

        // Act
        int actual = Day14.Simulate(paths);

        // Assert
        actual.ShouldBe(24);
    }

    [Fact]
    public async Task Y2022_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.GrainsOfSand.ShouldBe(832);
    }
}
