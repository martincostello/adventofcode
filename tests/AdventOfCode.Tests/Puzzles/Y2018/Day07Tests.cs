// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day07Tests : PuzzleTest
{
    public Day07Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public static void Y2018_Day07_Assemble_Returns_Correct_Solution()
    {
        // Arrange
        string[] coordinates =
        {
            "Step C must be finished before step A can begin.",
            "Step C must be finished before step F can begin.",
            "Step A must be finished before step B can begin.",
            "Step A must be finished before step D can begin.",
            "Step B must be finished before step E can begin.",
            "Step D must be finished before step E can begin.",
            "Step F must be finished before step E can begin.",
        };

        // Act
        string actual = Day07.Assemble(coordinates);

        // Assert
        actual.ShouldBe("CABDFE");
    }

    [Fact]
    public async Task Y2018_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.OrderOfAssembly.ShouldBe("BGJCNLQUYIFMOEZTADKSPVXRHW");
    }
}
