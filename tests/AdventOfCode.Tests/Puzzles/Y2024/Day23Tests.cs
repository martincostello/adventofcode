// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day23Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact(Skip = "Not implemented.")]
    public void Y2024_Day23_CountNetworks_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "kh-tc",
            "qp-kh",
            "de-cg",
            "ka-co",
            "yn-aq",
            "qp-ub",
            "cg-tb",
            "vc-aq",
            "tb-ka",
            "wh-tc",
            "yn-cg",
            "kh-ub",
            "ta-co",
            "de-co",
            "tc-td",
            "tb-wq",
            "wh-td",
            "ta-ka",
            "td-qp",
            "aq-cg",
            "wq-ub",
            "ub-vc",
            "de-ta",
            "wq-aq",
            "wq-vc",
            "wh-yn",
            "ka-de",
            "kh-ta",
            "co-tc",
            "wh-qp",
            "tb-vc",
            "td-yn",
        ];

        // Act
        int actual = Day23.CountNetworks(values);

        // Assert
        actual.ShouldBe(7);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2024_Day23_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day23>();

        // Assert
        puzzle.Solution.ShouldBe(Puzzle.Unsolved);
    }
}
