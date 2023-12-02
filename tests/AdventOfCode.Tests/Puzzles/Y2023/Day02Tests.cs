// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day02_Play_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
            "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
            "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
            "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
            "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
        ];

        // Act
        (int actualIdsSum, int actualPowersSum) = Day02.Play(values);

        // Assert
        actualIdsSum.ShouldBe(8);
        actualPowersSum.ShouldBe(2286);
    }

    [Fact]
    public async Task Y2023_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfPossibleSolutions.ShouldBe(2156);
        puzzle.SumOfPowers.ShouldBe(66909);
    }
}
