﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day09Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day09Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day09Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(new[] { "London to Dublin = 464" }, 464)]
    [InlineData(new[] { "London to Dublin = 464", "London to Belfast = 518", "Dublin to Belfast = 141" }, 605)]
    public static void Y2015_Day09_GetDistanceBetweenPoints_Returns_Correct_Value(string[] distances, int expected)
    {
        // Act
        int actual = Day09.GetDistanceBetweenPoints(distances, findLongest: false);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShortestDistance.ShouldBe(207);
        puzzle.LongestDistance.ShouldBe(804);
    }
}
