// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day17Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day17_Run_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "Register A: 729",
            "Register B: 0",
            "Register C: 0",
            string.Empty,
            "Program: 0,1,5,4,3,0",
        ];

        // Act
        string actual = Day17.Run(values);

        // Assert
        actual.ShouldBe("4,6,3,5,6,3,5,2,1,0");
    }

    [Fact]
    public async Task Y2024_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Output.ShouldBe("1,2,3,1,3,2,5,3,1");
    }
}
