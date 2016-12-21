// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day16Tests
    {
        [Theory]
        [InlineData("110010110100", 12, "100")]
        [InlineData("10000", 20, "01100")]
        public static void Y2016_Day16_GetDiskChecksum_Returns_Correct_Solution(string initial, int size, string expected)
        {
            // Act
            string actual = Day16.GetDiskChecksum(initial, size);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day16_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "10010000000110000", "272" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day16>(args);

            // Assert
            Assert.Equal("10010110010011110", puzzle.Checksum);
        }
    }
}
