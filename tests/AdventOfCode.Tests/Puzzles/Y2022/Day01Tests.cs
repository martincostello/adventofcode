// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2022_Day01_GetCalorieInventories_Returns_Correct_Values()
    {
        // Arrange
        string[] inventory = new[]
        {
            "1000",
            "2000",
            "3000",
            string.Empty,
            "4000",
            string.Empty,
            "5000",
            "6000",
            string.Empty,
            "7000",
            "8000",
            "9000",
            string.Empty,
            "10000",
        };

        // Act
        var actual = Day01.GetCalorieInventories(inventory);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBe(new[] { 6000, 4000, 11000, 24000, 10000 });
    }

    [Fact]
    public async Task Y2022_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MaximumCalories.ShouldBe(68775);
        puzzle.MaximumCaloriesForTop3.ShouldBe(202585);
    }
}
