// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day11Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day11_CountPaths_Returns_Correct_You_Value()
    {
        // Arrange
        string[] values =
        [
            "aaa: you hhh",
            "you: bbb ccc",
            "bbb: ddd eee",
            "ccc: ddd eee fff",
            "ddd: ggg",
            "eee: out",
            "fff: out",
            "ggg: out",
            "hhh: ccc fff iii",
            "iii: out",
        ];

        // Act
        (int actual, _) = Day11.CountPaths(values, TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldBe(5);
    }

    [Fact]
    public void Y2025_Day11_CountPaths_Returns_Correct_Server_Value()
    {
        // Arrange
        string[] values =
        [
            "svr: aaa bbb",
            "aaa: fft",
            "fft: ccc",
            "bbb: tty",
            "tty: ccc",
            "ccc: ddd eee",
            "ddd: hub",
            "hub: fff",
            "eee: dac",
            "dac: fff",
            "fff: ggg hhh",
            "ggg: out",
            "hhh: out",
        ];

        // Act
        (_, int actual) = Day11.CountPaths(values, TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldBe(2);
    }

    [Fact(Skip = "Too slow.")]
    public async Task Y2025_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution1.ShouldBe(788);
        puzzle.Solution2.ShouldBe(Puzzle.Unsolved);
    }
}
