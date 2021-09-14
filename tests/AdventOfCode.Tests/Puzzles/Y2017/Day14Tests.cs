// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

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
    public static void Y2017_Day14_GetSquaresUsed_Returns_Correct_Value()
    {
        // Arrange
        string key = "flqrgnkx";

        // Act
        int actual = Day14.GetSquaresUsed(key);

        // Assert
        actual.ShouldBe(8108);
    }

    [Fact]
    public async Task Y2017_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>("hwlqcszp");

        // Assert
        puzzle.SquaresUsed.ShouldBe(8304);
    }
}
