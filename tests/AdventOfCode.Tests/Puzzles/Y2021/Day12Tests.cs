// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day12Tests : PuzzleTest
{
    public Day12Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day12_Simulate_Returns_Correct_Value_1()
    {
        // Arrange
        string[] nodes =
        {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end",
        };

        // Act
        int actual = Day12.Navigate(nodes);

        // Assert
        actual.ShouldBe(10);
    }

    [Fact]
    public void Y2021_Day12_Simulate_Returns_Correct_Value_2()
    {
        // Arrange
        string[] nodes =
        {
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
        };

        // Act
        int actual = Day12.Navigate(nodes);

        // Assert
        actual.ShouldBe(19);
    }

    [Fact]
    public void Y2021_Day12_Simulate_Returns_Correct_Value_3()
    {
        // Arrange
        string[] nodes =
        {
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
        };

        // Act
        int actual = Day12.Navigate(nodes);

        // Assert
        actual.ShouldBe(226);
    }

    [Fact]
    public async Task Y2021_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.Count.ShouldBe(4413);
    }
}
