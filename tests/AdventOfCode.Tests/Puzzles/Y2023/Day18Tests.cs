// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day18Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day18_Dig_Returns_Correct_Value()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

        string[] plan =
        [
            "R 6 (#70c710)",
            "D 5 (#0dc571)",
            "L 2 (#5713f0)",
            "D 2 (#d2c081)",
            "R 2 (#59c680)",
            "D 2 (#411b91)",
            "L 5 (#8ceee2)",
            "U 2 (#caa173)",
            "L 1 (#1b58a2)",
            "U 2 (#caa171)",
            "R 2 (#7807d2)",
            "U 3 (#a77fa3)",
            "L 2 (#015232)",
            "U 2 (#7a21e3)",
        ];

        // Act
        (int actual, string visualization) = Day18.Dig(plan, cts.Token);

        Logger.WriteLine(visualization);

        // Assert
        actual.ShouldBe(62);
    }

    [Fact]
    public async Task Y2023_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Volume.ShouldBe(47139);
    }
}
