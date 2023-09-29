// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

#pragma warning disable SA1010

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2017_Day06_Debug_Returns_Correct_Value()
    {
        // Arrange
        int[] memory = [0, 2, 7, 0];

        // Act
        (int cycleCount, int loopSize) = Day06.Debug(memory);

        // Assert
        cycleCount.ShouldBe(5);
        loopSize.ShouldBe(4);
    }

    [Fact]
    public async Task Y2017_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.CycleCount.ShouldBe(3156);
        puzzle.LoopSize.ShouldBe(1610);
    }
}
