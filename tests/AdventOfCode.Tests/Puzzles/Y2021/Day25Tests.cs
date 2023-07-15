// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day25Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2021_Day25_ClearSeaFloor_Returns_Correct_Value()
    {
        // Arrange
        string[] map =
        {
            "v...>>.vv>",
            ".vv>>.vv..",
            ">>.>v>...v",
            ">>v>>.>.v.",
            "v>v.vv.v..",
            ">.>>..v...",
            ".vv..>.>v.",
            "v.v..>>v.v",
            "....v..v.>",
        };

        // Act
        int actual = Day25.ClearSeaFloor(map);

        // Assert
        actual.ShouldBe(58);
    }

    [Fact]
    public async Task Y2021_Day25_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day25>();

        // Assert
        puzzle.FirstStepWithNoMoves.ShouldBe(532);
    }
}
