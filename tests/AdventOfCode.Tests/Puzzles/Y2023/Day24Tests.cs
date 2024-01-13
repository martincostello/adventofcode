// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day24Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day24_Predict_Returns_Correct_Value()
    {
        // Arrange
        string[] hailstones =
        [
            "19, 13, 30 @ -2,  1, -2",
            "18, 19, 22 @ -1, -1, -2",
            "20, 25, 34 @ -2, -2, -4",
            "12, 31, 28 @ -1, -2, -1",
            "20, 19, 15 @  1, -5, -3",
        ];

        // Act
        int actual = Day24.Predict(hailstones, 7, 27);

        // Assert
        actual.ShouldBe(2);
    }

    [Fact(Skip = "Unsolved.")]
    public async Task Y2023_Day24_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Intersections.ShouldBe(-1);
    }
}
