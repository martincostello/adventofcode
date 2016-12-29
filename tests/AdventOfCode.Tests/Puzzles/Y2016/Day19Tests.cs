// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day19Tests
    {
        [Theory]
        [InlineData(5, 1, 3)]
        [InlineData(5, 2, 2)]
        public static void Y2016_Day19_FindElfThatGetsAllPresents_Returns_Correct_Solution(int count, int version, int expected)
        {
            // Act
            int actual = Day19.FindElfThatGetsAllPresents(count, version);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("5", "1", 3)]
        [InlineData("5", "2", 2)]
        [InlineData("3014387", "1", 1834471)]
        [InlineData("3014387", "2", 1420064)]
        public static void Y2016_Day19_Solve_Returns_Correct_Solution(string elves, string version, int expected)
        {
            // Arrange
            string[] args = new[] { elves, version };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day19>(args);

            // Assert
            Assert.Equal(expected, puzzle.ElfWithAllPresents);
        }
    }
}
