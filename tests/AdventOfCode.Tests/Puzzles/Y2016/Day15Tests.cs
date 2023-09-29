﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day15Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2016_Day15_FindTimeForCapsuleRelease_Returns_Correct_Solution()
    {
        // Arrange
        string[] input =
        [
            "Disc #1 has 5 positions; at time=0, it is at position 4.",
            "Disc #2 has 2 positions; at time=0, it is at position 1.",
        ];

        // Act
        int actual = Day15.FindTimeForCapsuleRelease(input);

        // Assert
        actual.ShouldBe(5);
    }

    [Fact]
    public async Task Y2016_Day15_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>();

        // Assert
        puzzle.TimeOfFirstButtonPress.ShouldBe(16824);
        puzzle.TimeOfFirstButtonPressWithExtraDisc.ShouldBe(3543984);
    }
}
