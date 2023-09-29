// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 36)]
    public void Y2021_Day12_Navigate_Returns_Correct_Value_1(int smallCaveVisitLimit, int expected)
    {
        // Arrange
        string[] nodes =
        [
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end",
        ];

        // Act
        int actual = Day12.Navigate(nodes, smallCaveVisitLimit);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, 19)]
    [InlineData(2, 103)]
    public void Y2021_Day12_Navigate_Returns_Correct_Value_2(int smallCaveVisitLimit, int expected)
    {
        // Arrange
        string[] nodes =
        [
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc",
        ];

        // Act
        int actual = Day12.Navigate(nodes, smallCaveVisitLimit);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, 226)]
    [InlineData(2, 3509)]
    public void Y2021_Day12_Navigate_Returns_Correct_Value_3(int smallCaveVisitLimit, int expected)
    {
        // Arrange
        string[] nodes =
        [
            "fs-end",
            "he-DX",
            "fs-he",
            "start-DX",
            "pj-DX",
            "end-zg",
            "zg-sl",
            "zg-pj",
            "pj-he",
            "RW-he",
            "fs-DX",
            "pj-RW",
            "zg-RW",
            "start-pj",
            "he-WI",
            "zg-he",
            "pj-fs",
            "start-RW",
        ];

        // Act
        int actual = Day12.Navigate(nodes, smallCaveVisitLimit);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.Count1.ShouldBe(4413);
        puzzle.Count2.ShouldBe(118803);
    }
}
