// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day22Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("easy", 953)]
    [InlineData("hard", 1289)]
    public void Y2015_Day22_Solve_Returns_Correct_Solution(string difficulty, int expected)
    {
        // Act
        int actual = Day22.Fight(difficulty);

        // Assert
        actual.ShouldBe(expected);
    }
}
