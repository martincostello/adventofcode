// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day06Tests : PuzzleTest
{
    public Day06Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day06_NavigateVents_Returns_Correct_Value()
    {
        // Arrange
        int[] fish = { 3, 4, 3, 1, 2 };

        // Act
        int actual = Day06.CountFish(fish, days: 80);

        // Assert
        actual.ShouldBe(5934);
    }

    [Fact]
    public async Task Y2021_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.FishCount.ShouldBe(377263);
    }
}
