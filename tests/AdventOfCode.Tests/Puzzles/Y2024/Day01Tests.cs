// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day01Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day01_ParseList_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "3   4",
            "4   3",
            "2   5",
            "1   3",
            "3   9",
            "3   3",
        ];

        // Act
        (int totalDistance, int similarity) = Day01.ParseList(values);

        // Assert
        totalDistance.ShouldBe(11);
        similarity.ShouldBe(31);
    }

    [Fact]
    public async Task Y2024_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalDistance.ShouldBe(1530215);
        puzzle.SimilarityScore.ShouldBe(26800609);
    }
}
