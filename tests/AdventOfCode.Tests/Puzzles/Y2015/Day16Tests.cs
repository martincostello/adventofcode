// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day16Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public async Task Y2015_Day16_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day16>();

        // Assert
        puzzle.AuntSueNumber.ShouldBe(373);
        puzzle.RealAuntSueNumber.ShouldBe(260);
    }
}
