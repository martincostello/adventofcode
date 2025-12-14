// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day10_Explore_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "89010123",
            "78121874",
            "87430965",
            "96549874",
            "45678903",
            "32019012",
            "01329801",
            "10456732",
        ];

        // Act
        (int actualScore, int actualRating) = Day10.Explore(values);

        // Assert
        actualScore.ShouldBe(36);
        actualRating.ShouldBe(81);
    }

    [Fact]
    public async Task Y2024_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.Solution1.ShouldBe(778);
        puzzle.Solution2.ShouldBe(1925);
    }
}
