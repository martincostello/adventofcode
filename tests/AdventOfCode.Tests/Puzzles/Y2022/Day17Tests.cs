// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day17"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day17Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day17Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day17Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day17_GetHeightOfTower_Returns_Correct_Value()
    {
        // Arrange
        string jets = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
        int rocks = 2022;

        // Act
        int actual = Day17.GetHeightOfTower(jets, rocks);

        // Assert
        actual.ShouldBe(3068);
    }

    [Fact]
    public async Task Y2022_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Height.ShouldBe(3135);
    }
}
