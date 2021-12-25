// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day24Tests : PuzzleTest
{
    public Day24Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public async Task Y2021_Day24_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>();

        // Assert
        puzzle.MaximumValidModelNumber.ShouldBe(92928914999991);
        puzzle.MinimumValidModelNumber.ShouldBe(91811211611981);
    }
}
