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
        [InlineData("ADVENT", 1, 6)]
        [InlineData("A(1x5)BC", 1, 7)]
        [InlineData("(3x3)XYZ", 1, 9)]
        [InlineData("A(2x2)BCD(2x2)EFG", 1, 11)]
        [InlineData("(6x1)(1x3)A", 1, 6)]
        [InlineData("X(8x2)(3x3)ABCY", 1, 18)]
        [InlineData("(3x3)XYZ", 2, 9)]
        [InlineData("X(8x2)(3x3)ABCY", 2, 20)]
        [InlineData("(27x12)(20x12)(13x14)(7x10)(1x12)A", 2, 241920)]
        [InlineData("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", 2, 445)]
        public static void Y2016_Day09_GetDecompressedLength_Returns_Correct_Solution(string data, int version, long expected)
        {
            // Act
            long actual = Day09.GetDecompressedLength(data, version);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day09_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day09>();

            // Assert
            Assert.Equal(98135L, puzzle.DecompressedLengthVersion1);
            Assert.Equal(10964557606L, puzzle.DecompressedLengthVersion2);
        }
    }
}
