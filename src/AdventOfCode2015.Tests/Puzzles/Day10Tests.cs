// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day10"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day10Tests
    {
        [Theory]
        [InlineData("1", "11")]
        [InlineData("11", "21")]
        [InlineData("21", "1211")]
        [InlineData("1211", "111221")]
        [InlineData("111221", "312211")]
        public static void Day10_AsLookAndSay(string value, string expected)
        {
            // Act
            string actual = Day10.AsLookAndSay(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day10_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "1321131112", "40" };
            Day10 target = new Day10();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(492982, target.Solution);

            // Arrange
            args = new[] { "1321131112", "50" };

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(6989950, target.Solution);
        }
    }
}
