// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day13Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day13_Play_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "Button A: X+94, Y+34",
            "Button B: X+22, Y+67",
            "Prize: X=8400, Y=5400",
            string.Empty,
            "Button A: X+26, Y+66",
            "Button B: X+67, Y+21",
            "Prize: X=12748, Y=12176",
            string.Empty,
            "Button A: X+17, Y+86",
            "Button B: X+84, Y+37",
            "Prize: X=7870, Y=6450",
            string.Empty,
            "Button A: X+69, Y+23",
            "Button B: X+27, Y+71",
            "Prize: X=18641, Y=10279",
        ];

        // Act
        long actual = Day13.Play(values, offset: 0, limit: 100);

        // Assert
        actual.ShouldBe(480);
    }

    [Fact]
    public async Task Y2024_Day13_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day13>();

        // Assert
        puzzle.Solution1.ShouldBe(37680);
        puzzle.Solution2.ShouldBe(87550094242995);
    }
}
