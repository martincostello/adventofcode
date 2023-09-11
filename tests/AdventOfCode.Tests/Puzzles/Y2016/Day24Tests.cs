// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day24Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2016_Day24_GetMinimumStepsToVisitLocations_Returns_Correct_Solution()
    {
        // Arrange
        string[] layout = new[]
        {
            "###########",
            "#0.1.....2#",
            "#.#######.#",
            "#4.......3#",
            "###########",
        };

        // Act
        int actual = Day24.GetMinimumStepsToVisitLocations(layout, returnToOrigin: false);

        // Assert
        actual.ShouldBe(14);
    }

    [Fact]
    public async Task Y2016_Day24_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>();

        // Assert
        puzzle.FewestStepsToVisitAllLocations.ShouldBe(502);
        puzzle.FewestStepsToVisitAllLocationsAndReturn.ShouldBe(724);
    }
}
