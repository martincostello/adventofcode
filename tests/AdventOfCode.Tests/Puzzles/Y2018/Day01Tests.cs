// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day01Tests
    {
        [Theory]
        [InlineData(new[] { "+1", "-2", "+3", "+1" }, 3)]
        [InlineData(new[] { "+1", "+1", "+1" }, 3)]
        [InlineData(new[] { "+1", "+1", "-2" }, 0)]
        [InlineData(new[] { "-1", "-2", "-3" }, -6)]
        public static void Y2018_Day01_CalculateFrequency_Returns_Correct_Solution(
            string[] sequence,
            int expected)
        {
            // Act
            int actual = Day01.CalculateFrequency(sequence);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2018_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(543, puzzle.Frequency);
        }
    }
}
