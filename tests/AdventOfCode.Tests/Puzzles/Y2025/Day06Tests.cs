// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day06_Solve_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "_",
        ];

        // Act
        int actual = Day06.Solve(values);

        // Assert
        actual.ShouldBe(Puzzle.Unsolved);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2025_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(Puzzle.Unsolved);
    }
}
