// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "rect 3x2", "rotate column x=1 by 1", "rotate row y=0 by 4", "rotate column x=1 by 1" }, 7, 3, 6)]
    public void Y2016_Day08_GetPixelsLit_Returns_Correct_Solution(string[] instructions, int width, int height, int expected)
    {
        // Act
        (int actual, _, _) = Day08.GetPixelsLit(instructions, width, height, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.PixelsLit.ShouldBe(121);
        puzzle.Code.ShouldBe("RURUCEOEIL");
    }
}
