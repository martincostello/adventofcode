// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day01_GetPassword_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "L68",
            "L30",
            "R48",
            "L5",
            "R60",
            "L55",
            "L1",
            "L99",
            "R14",
            "L82",
        ];

        // Act
        int password = Day01.GetPassword(values);

        // Assert
        password.ShouldBe(3);
    }

    [Fact]
    public async Task Y2025_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Password.ShouldBe(1102);
    }
}
