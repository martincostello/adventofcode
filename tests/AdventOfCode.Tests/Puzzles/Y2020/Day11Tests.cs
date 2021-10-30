// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day11Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day11Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day11Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(1, 37)]
    [InlineData(2, 26)]
    public void Y2020_Day11_GetOccupiedSeats_Returns_Correct_Value(int version, int expected)
    {
        // Arrange
        string[] layout = new[]
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL",
        };

        // Act
        (int actual, _) = Day11.GetOccupiedSeats(layout, version, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.OccupiedSeatsV1.ShouldBe(2108);
        puzzle.OccupiedSeatsV2.ShouldBe(1897);
    }
}
