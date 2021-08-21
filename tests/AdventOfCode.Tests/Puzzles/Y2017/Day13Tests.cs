// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class containing tests for the <see cref="Day13"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day13Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day13Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day13Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public static void Y2017_Day13_GetSeverityOfTrip_Returns_Correct_Value()
    {
        // Arrange
        string[] depthRanges = new[]
        {
            "0: 3",
            "1: 2",
            "4: 4",
            "6: 4",
        };

        // Act
        int actual = Day13.GetSeverityOfTrip(depthRanges);

        // Assert
        actual.ShouldBe(24);
    }

    [Fact]
    public static void Y2017_Day13_GetShortestDelayForNeverCaught_Returns_Correct_Value()
    {
        // Arrange
        string[] depthRanges = new[]
        {
            "0: 3",
            "1: 2",
            "4: 4",
            "6: 4",
        };

        // Act
        int actual = Day13.GetShortestDelayForNeverCaught(depthRanges);

        // Assert
        actual.ShouldBe(10);
    }

    [Fact]
    public async Task Y2017_Day13_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day13>();

        // Assert
        puzzle.Severity.ShouldBe(1612);
        puzzle.ShortestDelay.ShouldBe(3907994);
    }
}
