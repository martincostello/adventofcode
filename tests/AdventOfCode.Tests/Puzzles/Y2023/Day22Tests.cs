// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day22Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day22_Disintegrate_Returns_Correct_Value()
    {
        // Arrange
        string[] snapshot =
        [
            "1,0,1~1,2,1",
            "0,0,2~2,0,2",
            "0,2,3~2,2,3",
            "0,0,4~0,2,4",
            "2,0,5~2,2,5",
            "0,1,6~2,1,6",
            "1,1,8~1,1,9",
        ];

        // Act
        (int actualBricks, int actualSum) = Day22.Disintegrate(snapshot);

        // Assert
        actualBricks.ShouldBe(5);
        actualSum.ShouldBe(7);
    }

    [Fact]
    public async Task Y2023_Day22_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day22>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SafeBricks.ShouldBe(454);
        puzzle.MaximumChainReaction.ShouldBe(74287);
    }
}
