// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2018_Day06_GetLargestArea_Returns_Correct_Solution()
    {
        // Arrange
        string[] coordinates =
        {
            "1, 1",
            "1, 6",
            "8, 3",
            "3, 4",
            "5, 5",
            "8, 9",
        };

        // Act
        (int largestArea, int areaOfRegion) = Day06.GetLargestArea(coordinates, distanceLimit: 32);

        // Assert
        largestArea.ShouldBe(17);
        areaOfRegion.ShouldBe(16);
    }

    [Fact]
    public async Task Y2018_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.LargestNonInfiniteArea.ShouldBe(5626);
        puzzle.AreaOfRegion.ShouldBe(46554);
    }
}
