// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day20"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day20Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day20Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day20Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day20_FindGrove_Returns_Correct_Value()
    {
        // Arrange
        string[] values = new[]
        {
            "1",
            "2",
            "-3",
            "3",
            "-2",
            "0",
            "4",
        };

        // Act
        int actual = Day20.FindGrove(values);

        // Assert
        actual.ShouldBe(3);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2022_Day20_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day20>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfCoordinates.ShouldBeGreaterThan(8077);
        puzzle.SumOfCoordinates.ShouldBe(-1);
    }
}
