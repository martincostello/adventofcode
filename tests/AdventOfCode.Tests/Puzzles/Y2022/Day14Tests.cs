// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 24)]
    [InlineData(true, 93)]
    public void Y2022_Day14_Simulate_Returns_Correct_Value(bool hasFloor, int expected)
    {
        // Arrange
        string[] paths = new[]
        {
            "498,4 -> 498,6 -> 496,6",
            "503,4 -> 502,4 -> 502,9 -> 494,9",
        };

        // Act
        (int actual, _) = Day14.Simulate(paths, hasFloor);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.GrainsOfSandWithVoid.ShouldBe(832);
        puzzle.GrainsOfSandWithFloor.ShouldBe(27601);
    }
}
