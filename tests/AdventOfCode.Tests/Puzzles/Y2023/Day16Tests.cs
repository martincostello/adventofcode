// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day16Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 46)]
    [InlineData(true, 51)]
    public void Y2023_Day16_Energize_Returns_Correct_Value(bool optimize, int expected)
    {
        // Arrange
        string[] layout =
        [
            @".|...\....",
            @"|.-.\.....",
            @".....|-...",
            @"........|.",
            @"..........",
            @".........\",
            @"..../.\\..",
            @".-.-/..|..",
            @".|....-|.\",
            @"..//.|....",
        ];

        // Act
        (int actual, string visualization) = Day16.Energize(layout, optimize);

        // Assert
        Logger.WriteLine(visualization);
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day16_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day16>();

        // Assert
        puzzle.Solution1.ShouldBe(6906);
        puzzle.Solution2.ShouldBe(7330);
    }
}
