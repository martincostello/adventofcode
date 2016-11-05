// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day18"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day18Tests
    {
        [Fact]
        public static void Y2015_Day18_GetGridConfigurationAfterSteps()
        {
            // Arrange
            string[] initialState = new string[]
            {
                ".#.#.#",
                "...##.",
                "#....#",
                "..#...",
                "#.#..#",
                "####..",
            };

            int steps = 0;
            bool areCornerLightsBroken = false;

            // Act
            IList<string> actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(6, actual.Count);
            Assert.Equal(".#.#.#", actual[0]);
            Assert.Equal("...##.", actual[1]);
            Assert.Equal("#....#", actual[2]);
            Assert.Equal("..#...", actual[3]);
            Assert.Equal("#.#..#", actual[4]);
            Assert.Equal("####..", actual[5]);

            // Arrange
            steps = 4;

            // Act
            actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(6, actual.Count);
            Assert.Equal("......", actual[0]);
            Assert.Equal("......", actual[1]);
            Assert.Equal("..##..", actual[2]);
            Assert.Equal("..##..", actual[3]);
            Assert.Equal("......", actual[4]);
            Assert.Equal("......", actual[5]);

            // Arrange
            steps = 5;
            areCornerLightsBroken = true;

            // Act
            actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(6, actual.Count);
            Assert.Equal("##.###", actual[0]);
            Assert.Equal(".##..#", actual[1]);
            Assert.Equal(".##...", actual[2]);
            Assert.Equal(".##...", actual[3]);
            Assert.Equal("#.#...", actual[4]);
            Assert.Equal("##...#", actual[5]);
        }

        [Theory]
        [InlineData("false", 814)]
        [InlineData("true", 924)]
        public static void Y2015_Day18_Solve_Returns_Correct_Solution(string areCornerLightsBroken, int lightsIlluminated)
        {
            // Arrange
            string[] args = new[] { "100", areCornerLightsBroken };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day18>(args);

            // Assert
            Assert.Equal(lightsIlluminated, puzzle.LightsIlluminated);
        }
    }
}
