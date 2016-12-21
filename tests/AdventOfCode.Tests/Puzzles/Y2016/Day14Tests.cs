// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day14"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day14Tests
    {
        [Theory]
        [InlineData("abc", 64, false, 22728)]
        [InlineData("abc", 64, true, 22551)]
        public static void Y2016_Day14_GetOneTimePadKeyIndex_Returns_Correct_Solution(string salt, int ordinal, bool useKeyStretching, int expected)
        {
            // Act
            int actual = Day14.GetOneTimePadKeyIndex(salt, ordinal, useKeyStretching);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day14_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "ihaygndm" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day14>(args);

            // Assert
            Assert.Equal(15035, puzzle.IndexOfKey64);
            Assert.Equal(19968, puzzle.IndexOfKey64WithStretching);
        }
    }
}
