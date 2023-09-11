// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day22Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public async Task Y2016_Day22_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day22>();

        // Assert
        puzzle.ViableNodePairs.ShouldBe(985);
        puzzle.MinimumStepsToExtract.ShouldBe(179);
    }
}
