// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day08Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day08Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day08Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day08_CountVisibleTrees_Returns_Correct_Value()
    {
        // Arrange
        string[] grid = new[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };

        // Act
        (int actualCount, int actualScore) = Day08.CountVisibleTrees(grid);

        // Assert
        actualCount.ShouldBe(21);
        actualScore.ShouldBe(8);
    }

    [Fact]
    public async Task Y2022_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.VisibleTrees.ShouldBe(1763);
        puzzle.MaximumScenicScore.ShouldBe(671160);
    }
}
