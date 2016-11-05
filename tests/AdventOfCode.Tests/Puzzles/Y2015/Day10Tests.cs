// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
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
        public static void Y2015_Day10_AsLookAndSay(string value, string expected)
        {
            // Act
            string actual = Day10.AsLookAndSay(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("40", 492982)]
        [InlineData("50", 6989950)]
        public static void Y2015_Day10_Solve_Returns_Correct_Solution(string length, int expected)
        {
            // Arrange
            string[] args = new[] { "1321131112", length };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day10>(args);

            // Assert
            Assert.Equal(expected, puzzle.Solution);
        }
    }
}
