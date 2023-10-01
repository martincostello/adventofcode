// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day17Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2015_Day17_GetContainerCombinationCount()
    {
        // Arrange
        int volume = 25;
        int[] containerVolumes = [20, 15, 10, 5, 5];

        // Act
        var result = Day17.GetContainerCombinations(volume, containerVolumes);

        // Assert
        result.Count.ShouldBe(4);
        result.GroupBy((p) => p.Count).OrderBy((p) => p.Key).First().Count().ShouldBe(3);
    }

    [Fact]
    public async Task Y2015_Day17_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day17>("150");

        // Assert
        puzzle.Combinations.ShouldBe(1304);
        puzzle.CombinationsWithMinimumContainers.ShouldBe(18);
    }
}
