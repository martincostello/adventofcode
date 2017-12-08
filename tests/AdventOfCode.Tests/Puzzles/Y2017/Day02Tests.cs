// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day02Tests
    {
        [Theory]
        [InlineData(new[] { 5, 1, 9, 5 }, 8)]
        [InlineData(new[] { 7, 5, 3 }, 4)]
        [InlineData(new[] { 2, 4, 6, 8 }, 6)]
        public static void Y2017_Day02_ComputeDifference_Returns_Correct_Solution(int[] row, int expected)
        {
            // Act
            int actual = Day02.ComputeDifference(row);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day02_CalculateChecksum_Returns_Correct_Solution()
        {
            // Arrange
            var spreadsheet = new[]
            {
                new[] { 5, 1, 9, 5 },
                new[] { 7, 5, 3 },
                new[] { 2, 4, 6, 8 }
            };

            // Act
            int actual = Day02.CalculateChecksum(spreadsheet);

            // Assert
            Assert.Equal(18, actual);
        }

        [Fact]
        public static void Y2017_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day02>();

            // Assert
            Assert.Equal(36174, puzzle.Checksum);
        }
    }
}
