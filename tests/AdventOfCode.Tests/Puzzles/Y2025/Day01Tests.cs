// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 3)]
    [InlineData(true, 6)]
    public void Y2025_Day01_GetPassword_Returns_Correct_Value(bool useMethod0x434C49434B, int expected)
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
        int password = Day01.GetPassword(values, useMethod0x434C49434B);

        // Assert
        password.ShouldBe(expected);
    }

    [Theory]
    [InlineData("L1000", 10)]
    [InlineData("L1001", 10)]
    [InlineData("R1000", 10)]
    [InlineData("R1001", 10)]
    public void Y2025_Day01_GetPassword_Returns_Correct_Value_After_Rotation(string rotation, int expected)
    {
        // Act
        int password = Day01.GetPassword([rotation], useMethod0x434C49434B: true);

        // Assert
        password.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2025_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.Solution1.ShouldBe(1102);
        puzzle.Solution2.ShouldBe(6175);
    }
}
