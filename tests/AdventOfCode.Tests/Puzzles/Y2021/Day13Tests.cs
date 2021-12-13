// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day13Tests : PuzzleTest
{
    public Day13Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(0, 18)]
    [InlineData(1, 17)]
    [InlineData(2, 16)]
    public void Y2021_Day13_Navigate_Returns_Correct_Value(int folds, int expected)
    {
        // Arrange
        string[] instructions =
        {
            "6,10",
            "0,14",
            "9,10",
            "0,3",
            "10,4",
            "4,11",
            "6,0",
            "6,12",
            "4,1",
            "0,13",
            "10,12",
            "3,4",
            "3,0",
            "8,4",
            "1,10",
            "2,14",
            "8,10",
            "9,0",
            string.Empty,
            "fold along y=7",
            "fold along x=5",
        };

        // Act
        (int actual, _) = Day13.Fold(instructions, folds);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day13_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day13>();

        // Assert
        puzzle.DotCountAfterFold1.ShouldBe(647);
        puzzle.ActivationCode.ShouldBe("HEJHJRCJ");
    }
}
