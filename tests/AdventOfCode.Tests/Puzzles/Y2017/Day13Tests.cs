// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day13"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day13Tests
    {
        [Fact]
        public static void Y2017_Day13_GetSeverityOfTrip_Returns_Correct_Value()
        {
            // Arrange
            var depthRanges = new[]
            {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4",
            };

            // Act
            int actual = Day13.GetSeverityOfTrip(depthRanges);

            // Assert
            Assert.Equal(24, actual);
        }

        [Fact]
        public static void Y2017_Day13_GetShortestDelayForNeverCaught_Returns_Correct_Value()
        {
            // Arrange
            var depthRanges = new[]
            {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4",
            };

            // Act
            int actual = Day13.GetShortestDelayForNeverCaught(depthRanges);

            // Assert
            Assert.Equal(10, actual);
        }

        [Fact]
        public static void Y2017_Day13_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day13>();

            // Assert
            Assert.Equal(1612, puzzle.Severity);
            Assert.Equal(3907994, puzzle.ShortestDelay);
        }
    }
}
