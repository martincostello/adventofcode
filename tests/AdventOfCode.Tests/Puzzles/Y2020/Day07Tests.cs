// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day07_GetBagColorsThatCouldContainColor_Returns_Correct_Value()
    {
        // Arrange
        string color = "shiny gold";
        string[] values = new[]
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags.",
        };

        // Act
        int actual = Day07.GetBagColorsThatCouldContainColor(values, color);

        // Assert
        actual.ShouldBe(4);
    }

    [Fact]
    public void Y2020_Day07_GetInsideBagCount_Returns_Correct_Value()
    {
        // Arrange
        string color = "shiny gold";
        string[] values = new[]
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags.",
        };

        // Act
        int actual = Day07.GetInsideBagCount(values, color);

        // Assert
        actual.ShouldBe(32);
    }

    [Fact]
    public async Task Y2020_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>("shiny gold");

        // Assert
        puzzle.BagColorsThatCanContainColor.ShouldBe(179);
        puzzle.BagsInsideBag.ShouldBe(18925);
    }
}
