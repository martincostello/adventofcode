// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day11Tests
    {
        [Theory]
        [InlineData("abcdefgh", "abcdffaa")]
        [InlineData("ghijklmn", "ghjaabcc")]
        public static void Day11_GenerateNextPassword(string current, string expected)
        {
            // Act
            string actual = Day11.GenerateNextPassword(current);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("hijklmmn", false)]
        [InlineData("abbceffg", true)]
        [InlineData("abbcegjk", false)]
        public static void Day11_HasMoreThanOneDistinctPairOfLetters(string value, bool expected)
        {
            // Act
            bool actual = Day11.HasMoreThanOneDistinctPairOfLetters(value?.ToCharArray());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day11_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "cqjxjnds" };
            Day11 target = new Day11();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal("cqjxxyzz", target.NextPassword);

            // Arrange
            args = new[] { target.NextPassword };

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal("cqkaabcc", target.NextPassword);
        }
    }
}
