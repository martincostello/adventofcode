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
        [InlineData(9, new[] { "5-8", "0-2", "4-7" }, 3, 2)]
        public static void Y2016_Day20_GetLowestNonblockedIP_Returns_Correct_Solution(uint maxValue, string[] blacklist, uint expectedIP, uint expectedCount)
        {
            // Act
            uint count;
            uint address = Day20.GetLowestNonblockedIP(maxValue, blacklist, out count);

            // Assert
            Assert.Equal(expectedIP, address);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public static void Y2016_Day20_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day20>();

            // Assert
            Assert.Equal(22887907u, puzzle.LowestNonblockedIP);
            Assert.Equal(109u, puzzle.AllowedIPCount);
        }
    }
}
