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

        [Theory]
        [InlineData(new[] { "+1", "-2", "+3", "+1", "+1", "-2" }, 2)]
        [InlineData(new[] { "+1", "-1" }, 0)]
        [InlineData(new[] { "+3", "+3", "+4", "-2", "-4" }, 10)]
        [InlineData(new[] { "-6", "+3", "+8", "+5", "-6" }, 5)]
        [InlineData(new[] { "+7", "+7", "-2", "-7", "-4" }, 14)]
        public static void Y2018_Day01_CalculateFrequencyWithRepetition_Returns_Correct_Solution(
            string[] sequence,
            int expected)
        {
            // Act
            int actual = Day01.CalculateFrequencyWithRepetition(sequence).firstRepeat;

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
            Assert.Equal(621, puzzle.FirstRepeatedFrequency);
        }
    }
}
