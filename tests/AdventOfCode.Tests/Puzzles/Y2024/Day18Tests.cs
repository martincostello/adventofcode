// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day18Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2024_Day18_Simulate_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "5,4",
            "4,2",
            "4,5",
            "3,0",
            "2,1",
            "6,3",
            "2,4",
            "1,5",
            "0,6",
            "3,3",
            "2,6",
            "5,1",
            "1,2",
            "5,5",
            "2,5",
            "6,5",
            "1,4",
            "0,4",
            "6,4",
            "1,1",
            "6,1",
            "1,0",
            "0,5",
            "1,6",
            "2,0",
        ];

        using var cts = Timeout();

        // Act
        (int actualSteps, string actualCoordinate) = Day18.Simulate(values, size: 7, ticks: 12, cts.Token);

        // Assert
        actualSteps.ShouldBe(22);
        actualCoordinate.ShouldBe("6,1");
    }

    [Fact]
    public async Task Y2024_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MinimumSteps.ShouldBe(308);
        puzzle.BlockingByte.ShouldBe("46,28");
    }
}
