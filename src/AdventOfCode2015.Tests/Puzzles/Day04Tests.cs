// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day04Tests
    {
        [Theory]
        [InlineData("abcdef", 5, 609043)]
        [InlineData("pqrstuv", 5, 1048970)]
        public static void Day04_IsStringNiceV1(string secretKey, int zeroes, int expected)
        {
            // Act
            int actual = Day04.GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day04_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "iwrupvqb", "5" };
            Day04 target = new Day04();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(346386, target.LowestZeroHash);

            // Arrange
            args = new[] { "iwrupvqb", "6" };

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(9958218, target.LowestZeroHash);
        }
    }
}
