// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_Simple()
    {
        // Arrange
        string[] sketch =
        [
            ".....",
            ".S-7.",
            ".|.|.",
            ".L-J.",
            ".....",
        ];

        // Act
        int actual = Day10.Walk(sketch, CancellationToken.None);

        // Assert
        actual.ShouldBe(4);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_Complex()
    {
        // Arrange
        string[] sketch =
        [
            "..F7.",
            ".FJ|.",
            "SJ.L7",
            "|F--J",
            "LJ...",
        ];

        // Act
        int actual = Day10.Walk(sketch, CancellationToken.None);

        // Assert
        actual.ShouldBe(8);
    }

    [Fact]
    public async Task Y2023_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Steps.ShouldBeGreaterThan(1842);
        puzzle.Steps.ShouldBe(-1);
    }
}
