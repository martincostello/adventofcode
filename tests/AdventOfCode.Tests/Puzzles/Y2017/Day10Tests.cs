// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day10"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day10Tests
    {
        [Theory]
        [InlineData(5, new[] { 3, 4, 1, 5 }, 12)]
        public static void Y2017_Day10_FindProductOfFirstTwoHashElements_Returns_Correct_Value(
            int size,
            int[] lengths,
            int expected)
        {
            // Act
            int actual = Day10.FindProductOfFirstTwoHashElements(size, lengths);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "a2582a3a0e66e6e86e3812dcb672a272")]
        [InlineData("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd")]
        [InlineData("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d")]
        [InlineData("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e")]
        public static void Y2017_Day10_ComputeHash_Returns_Correct_Value(string asciiBytes, string expected)
        {
            // Act
            string actual = Day10.ComputeHash(asciiBytes);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day10_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day10>();

            // Assert
            Assert.Equal(11413, puzzle.ProductOfFirstTwoElements);
            Assert.Equal("7adfd64c2a03a4968cf708d1b7fd418d", puzzle.DenseHash);
        }
    }
}
