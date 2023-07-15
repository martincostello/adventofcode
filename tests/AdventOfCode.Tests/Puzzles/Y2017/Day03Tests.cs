// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("1", 0)]
    [InlineData("12", 3)]
    [InlineData("23", 2)]
    [InlineData("1024", 31)]
    [InlineData("312051", 430)]
    public async Task Y2017_Day03_Solve_Returns_Correct_Solution_For_Steps(string square, int expected)
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>(square);

        // Assert
        puzzle.Steps.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day03_Solve_Returns_Correct_Solution_For_Storage()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>("312051");

        // Assert
        puzzle.Steps.ShouldBe(430);
        puzzle.FirstStorageLargerThanInput.ShouldBe(312453);
    }
}
