// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day03_Solve_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "467..114..",
            "...*......",
            "..35..633.",
            "......#...",
            "617*......",
            ".....+.58.",
            "..592.....",
            "......755.",
            "...$.*....",
            ".664.598..",
        ];

        // Act
        (int actualSumParts, int actualSumGears) = Day03.Solve(values);

        // Assert
        actualSumParts.ShouldBe(4361);
        actualSumGears.ShouldBe(467835);
    }

    [Fact]
    public async Task Y2023_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfPartNumbers.ShouldBe(535351);
        puzzle.SumOfGearRatios.ShouldBe(87287096);
    }
}
