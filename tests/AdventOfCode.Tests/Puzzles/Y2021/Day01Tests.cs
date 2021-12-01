// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day01Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day01Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day01Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(new int[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 }, 7)]
    public void Y2021_Day01_Get2020Product_Returns_Correct_Value(int[] depthMeasurements, int expected)
    {
        // Act
        int actual = Day01.GetDepthMeasurementIncreases(depthMeasurements);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.DepthIncreases.ShouldBe(1532);
    }
}
