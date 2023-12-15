// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day19Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(5, 1, 3)]
    [InlineData(5, 2, 2)]
    public static void Y2016_Day19_FindElfThatGetsAllPresents_Returns_Correct_Solution(int count, int version, int expected)
    {
        // Act
        int actual = Day19.FindElfThatGetsAllPresents(count, version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("5", 3, 2)]
    [InlineData("3014387", 1834471, 1420064)]
    public async Task Y2016_Day19_Solve_Returns_Correct_Solution(string elves, int expectedV1, int expectedV2)
    {
        // Arrange
        string[] args = [elves];

        // Act
        var puzzle = await SolvePuzzleAsync<Day19>(args);

        // Assert
        puzzle.ElfWithAllPresentsV1.ShouldBe(expectedV1);
        puzzle.ElfWithAllPresentsV2.ShouldBe(expectedV2);
    }
}
