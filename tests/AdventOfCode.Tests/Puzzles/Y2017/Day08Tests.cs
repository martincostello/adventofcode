﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2017_Day08_FindHighestRegisterValueAtEnd_Returns_Correct_Value()
    {
        // Arrange
        string[] instructions = new[]
        {
            "b inc 5 if a > 1",
            "a inc 1 if b < 5",
            "c dec -10 if a >= 1",
            "c inc -20 if c == 10",
        };

        // Act
        int actual = Day08.FindHighestRegisterValueAtEnd(instructions);

        // Assert
        actual.ShouldBe(1);
    }

    [Fact]
    public static void Y2017_Day08_FindHighestRegisterValueDuring_Returns_Correct_Value()
    {
        // Arrange
        string[] instructions = new[]
        {
            "b inc 5 if a > 1",
            "a inc 1 if b < 5",
            "c dec -10 if a >= 1",
            "c inc -20 if c == 10",
        };

        // Act
        int actual = Day08.FindHighestRegisterValueDuring(instructions);

        // Assert
        actual.ShouldBe(10);
    }

    [Fact]
    public async Task Y2017_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.HighestRegisterValueAtEnd.ShouldBe(7296);
        puzzle.HighestRegisterValueDuring.ShouldBe(8186);
    }
}
