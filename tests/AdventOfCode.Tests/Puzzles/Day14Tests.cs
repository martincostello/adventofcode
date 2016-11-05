// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day14"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day14Tests
    {
        [Theory]
        [InlineData(1, 14, 10, 127, 14)]
        [InlineData(1, 16, 11, 162, 16)]
        [InlineData(11, 14, 10, 127, 140)]
        [InlineData(11, 16, 11, 162, 176)]
        [InlineData(12, 14, 10, 127, 140)]
        [InlineData(12, 16, 11, 162, 176)]
        [InlineData(1000, 14, 10, 127, 1120)]
        [InlineData(1000, 16, 11, 162, 1056)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Naming",
            "CA1702:CompoundWordsShouldBeCasedCorrectly",
            MessageId = "AfterTime",
            Justification = "'After time' is correct.")]
        public static void Day14_GetDistanceAfterTimeIndex(int timeIndex, int speed, int activityPeriod, int restPeriod, int expected)
        {
            // Arrange
            var target = new Day14.FlightData()
            {
                MaximumActivityPeriod = activityPeriod,
                MaximumSpeed = speed,
                RestPeriod = restPeriod,
            };

            // Act
            int actual = target.GetDistanceAfterTimeIndex(timeIndex);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day14_GetMaximumDistanceOfFastestReindeer()
        {
            // Arrange
            string[] flightData = new[]
            {
                "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
                "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.",
            };

            int timeIndex = 1000;

            // Act
            int actual = Day14.GetMaximumDistanceOfFastestReindeer(flightData, timeIndex);

            // Assert
            Assert.Equal(1120, actual);
        }

        [Fact]
        public static void Day14_GetMaximumPointsOfFastestReindeer()
        {
            // Arrange
            string[] flightData = new[]
            {
                "Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
                "Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.",
            };

            int timeIndex = 1000;

            // Act
            int actual = Day14.GetMaximumPointsOfFastestReindeer(flightData, timeIndex);

            // Assert
            Assert.Equal(689, actual);
        }

        [Fact]
        public static void Day14_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "2503" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day14>(args);

            // Assert
            Assert.Equal(2655, puzzle.MaximumReindeerDistance);
            Assert.Equal(1059, puzzle.MaximumReindeerPoints);
        }
    }
}
