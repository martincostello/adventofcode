// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact(Skip = "Not implemented.")]
    public static void Y2016_Day11_GetMinimumStepsForAssembly_Returns_Correct_Solution()
    {
        // Arrange
        string[] initialState =
        [
            "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.",
            "The second floor contains a hydrogen generator.",
            "The third floor contains a lithium generator.",
            "The fourth floor contains nothing relevant.",
        ];

        // Act
        int actual = Day11.GetMinimumStepsForAssembly(initialState);

        // Assert
        actual.ShouldBe(11);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2016_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.MinimumStepsForAssembly.ShouldBe(47);
    }
}
