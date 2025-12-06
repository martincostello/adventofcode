// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day06_SolveWorksheet_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  ",
        ];

        // Act
        long actual = Day06.SolveWorksheet(values);

        // Assert
        actual.ShouldBe(4277556);
    }

    [Fact]
    public async Task Y2025_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.GrandTotal.ShouldBe(5524274308182);
    }
}
