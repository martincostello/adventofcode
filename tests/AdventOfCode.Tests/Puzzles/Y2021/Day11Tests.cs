// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day11Tests : PuzzleTest
{
    public Day11Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day11_Simulate_Returns_Correct_Value_1()
    {
        // Arrange
        string[] grid =
        {
            "11111",
            "19991",
            "19191",
            "19991",
            "11111",
        };

        // Act
        (int actual, _) = Day11.Simulate(grid, steps: 2);

        // Assert
        actual.ShouldBe(9);
    }

    [Fact]
    public void Y2021_Day11_Simulate_Returns_Correct_Value_2()
    {
        // Arrange
        string[] grid =
        {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526",
        };

        // Act
        (int actual, _) = Day11.Simulate(grid, steps: 100);

        // Assert
        actual.ShouldBe(1656);
    }

    [Fact]
    public void Y2021_Day11_Simulate_Returns_Correct_Value_3()
    {
        // Arrange
        string[] grid =
        {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526",
        };

        // Act
        (_, int actual) = Day11.Simulate(grid, steps: int.MaxValue);

        // Assert
        actual.ShouldBe(195);
    }

    [Fact]
    public async Task Y2021_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.Flashes100.ShouldBe(1739);
        puzzle.IterationOfFirstSynchronizedFlash.ShouldBe(324);
    }
}
