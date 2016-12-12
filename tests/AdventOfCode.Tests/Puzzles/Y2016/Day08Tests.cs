// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day08Tests
    {
        [Theory]
        [InlineData(new[] { "rect 3x2", "rotate column x=1 by 1", "rotate row y=0 by 4", "rotate column x=1 by 1" }, 7, 3, 6)]
        public static void Y2016_Day08_GetPixelsLit_Returns_Correct_Solution(string[] instructions, int width, int height, int expected)
        {
            // Act
            int actual = Day08.GetPixelsLit(instructions, width, height);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day08_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day08>();

            // Assert
            Assert.Equal(121, puzzle.PixelsLit);
        }
    }
}
