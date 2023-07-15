// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
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
    public static void Y2015_Day14_GetDistanceAfterTimeIndex(int timeIndex, int speed, int activityPeriod, int restPeriod, int expected)
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
        actual.ShouldBe(expected);
    }

    [Fact]
    public static void Y2015_Day14_GetMaximumDistanceOfFastestReindeer()
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
        actual.ShouldBe(1120);
    }

    [Fact]
    public static void Y2015_Day14_GetMaximumPointsOfFastestReindeer()
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
        actual.ShouldBe(689);
    }

    [Fact]
    public async Task Y2015_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>("2503");

        // Assert
        puzzle.MaximumReindeerDistance.ShouldBe(2655);
        puzzle.MaximumReindeerPoints.ShouldBe(1059);
    }
}
