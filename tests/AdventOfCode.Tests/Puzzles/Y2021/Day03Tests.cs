﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day03Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day03Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day03Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day03_GetPowerConsumption_Returns_Correct_Value()
    {
        // Arrange
        string[] diagnosticReport =
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        // Act
        int actual = Day03.GetPowerConsumption(diagnosticReport);

        // Assert
        actual.ShouldBe(198);
    }

    [Fact]
    public async Task Y2021_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.PowerConsumption.ShouldBe(3633500);
    }
}
