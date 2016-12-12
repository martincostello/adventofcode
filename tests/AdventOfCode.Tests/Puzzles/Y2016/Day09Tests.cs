// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day09Tests
    {
        [Theory]
        [InlineData("ADVENT", "ADVENT")]
        [InlineData("A(1x5)BC", "ABBBBBC")]
        [InlineData("(3x3)XYZ", "XYZXYZXYZ")]
        [InlineData("A(2x2)BCD(2x2)EFG", "ABCBCDEFEFG")]
        [InlineData("(6x1)(1x3)A", "(1x3)A")]
        [InlineData("X(8x2)(3x3)ABCY", "X(3x3)ABC(3x3)ABCY")]
        public static void Y2016_Day09_Decompress_Returns_Correct_Solution(string data, string expected)
        {
            // Act
            string actual = Day09.Decompress(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day09_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day09>();

            // Assert
            Assert.Equal(98135, puzzle.DecompressedLength);
        }
    }
}
