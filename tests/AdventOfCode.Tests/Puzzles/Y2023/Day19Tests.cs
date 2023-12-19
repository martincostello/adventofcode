// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day19Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day19_Run_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "px{a<2006:qkq,m>2090:A,rfg}",
            "pv{a>1716:R,A}",
            "lnx{m>1548:A,A}",
            "rfg{s<537:gd,x>2440:R,A}",
            "qs{s>3448:A,lnx}",
            "qkq{x<1416:A,crn}",
            "crn{x>2662:A,R}",
            "in{s<1351:px,qqz}",
            "qqz{s>2770:qs,m<1801:hdj,R}",
            "gd{a>3333:R,R}",
            "hdj{m>838:A,pv}",
            string.Empty,
            "{x=787,m=2655,a=1222,s=2876}",
            "{x=1679,m=44,a=2067,s=496}",
            "{x=2036,m=264,a=79,s=2244}",
            "{x=2461,m=1339,a=466,s=291}",
            "{x=2127,m=1623,a=2188,s=1013}",
        ];

        // Act
        (int actualSum, long actualCombinations) = Day19.Run(values);

        // Assert
        actualSum.ShouldBe(19114);
        actualCombinations.ShouldBe(167409079868000);
    }

    [Fact]
    public async Task Y2023_Day19_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day19>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.RatingNumbersSum.ShouldBe(398527);
        puzzle.CombinationsAccepted.ShouldBe(-1);
    }
}
