// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day20"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day20Tests
    {
        [Theory]
        [InlineData(new[] { "5-8", "0-2", "4-7" }, 3)]
        public static void Y2016_Day20_GetLowestNonblockedIP_Returns_Correct_Solution(string[] blacklist, uint expected)
        {
            // Act
            uint actual = Day20.GetLowestNonblockedIP(blacklist);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day20_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day20>();

            // Assert
            Assert.Equal(22887907u, puzzle.LowestNonblockedIP);
        }
    }
}
