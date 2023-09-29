// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 150)]
    [InlineData(true, 900)]
    public void Y2021_Day02_NavigateCourse_Returns_Correct_Value(bool useAim, int expected)
    {
        // Arrange
        string[] course =
        [
            "forward 5",
            "down 5",
            "forward 8",
            "up 3",
            "down 8",
            "forward 2",
        ];

        // Act
        int actual = Day02.NavigateCourse(course, useAim);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ProductOfFinalPosition.ShouldBe(2150351);
        puzzle.ProductOfFinalPositionWithAim.ShouldBe(1842742223);
    }
}
