// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day05Tests
    {
        [Theory]
        [InlineData("abc", "18f47a30")]
        public static void Y2016_Day05_GeneratePassword_Returns_Correct_Solution(string doorId, string expected)
        {
            // Act
            string actual = Day05.GeneratePassword(doorId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day05_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "wtnhxymk" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day05>(args);

            // Assert
            Assert.Equal("2414bc77", puzzle.Password);
        }
    }
}
