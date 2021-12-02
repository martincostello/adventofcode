// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day02Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day02Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day02Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day02_NavigateCourse_Returns_Correct_Value()
    {
        // Arrange
        string[] course =
        {
            "forward 5",
            "down 5",
            "forward 8",
            "up 3",
            "down 8",
            "forward 2",
        };

        // Act
        int actual = Day02.NavigateCourse(course);

        // Assert
        actual.ShouldBe(150);
    }

    [Fact]
    public async Task Y2021_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ProductOfFinalPosition.ShouldBe(2150351);
    }
}
