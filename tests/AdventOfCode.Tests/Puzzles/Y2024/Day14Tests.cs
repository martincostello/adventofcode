// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day14_Simulate_Returns_Correct_Value()
    {
        // Arrange
        var bounds = new Rectangle(0, 0, 11, 7);
        int seconds = 100;

        string[] values =
        [
            "p=0,4 v=3,-3",
            "p=6,3 v=-1,-3",
            "p=10,3 v=-1,2",
            "p=2,0 v=2,-1",
            "p=0,0 v=1,3",
            "p=3,0 v=-2,-2",
            "p=7,6 v=-1,-3",
            "p=3,0 v=-1,-2",
            "p=9,3 v=2,3",
            "p=7,3 v=-1,2",
            "p=2,4 v=2,-3",
            "p=9,5 v=-3,-3",
        ];

        // Act
        int actual = Day14.Simulate(values, bounds, seconds);

        // Assert
        actual.ShouldBe(12);
    }

    [Fact]
    public async Task Y2024_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SafetyFactor.ShouldBe(231782040);
    }
}
