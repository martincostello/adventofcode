// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day12_GetDistanceTravelled_Returns_Correct_Value()
    {
        // Arrange
        string[] values = ["F10", "N3", "F7", "R90", "F11"];

        // Act
        int actual = Day12.GetDistanceTravelled(values);

        // Assert
        actual.ShouldBe(25);
    }

    [Fact]
    public void Y2020_Day12_GetDistanceTravelledWithWaypoint_Returns_Correct_Value()
    {
        // Arrange
        string[] values = ["F10", "N3", "F7", "R90", "F11"];

        // Act
        int actual = Day12.GetDistanceTravelledWithWaypoint(values);

        // Assert
        actual.ShouldBe(286);
    }

    [Fact]
    public async Task Y2020_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ManhattanDistance.ShouldBe(439);
        puzzle.ManhattanDistanceWithWaypoint.ShouldBe(12385);
    }
}
