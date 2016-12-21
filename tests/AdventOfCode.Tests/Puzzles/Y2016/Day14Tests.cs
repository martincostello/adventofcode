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
        [InlineData("abc", 64, 22728)]
        public static void Y2016_Day14_GetOneTimePadKey_Returns_Correct_Solution(string salt, int ordinal, int expected)
        {
            // Act
            int actual = Day14.GetOneTimePadKeyIndex(salt, ordinal);

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
        }
    }
}
