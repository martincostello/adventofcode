// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day18"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day18Tests
    {
        [Fact]
        public static void Day18_GetGridConfigurationAfterSteps()
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

            // Act
            IList<string> actual = Day18.GetGridConfigurationAfterSteps(initialState, steps);

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
            actual = Day18.GetGridConfigurationAfterSteps(initialState, steps);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(6, actual.Count);
            Assert.Equal("......", actual[0]);
            Assert.Equal("......", actual[1]);
            Assert.Equal("..##..", actual[2]);
            Assert.Equal("..##..", actual[3]);
            Assert.Equal("......", actual[4]);
            Assert.Equal("......", actual[5]);
        }

        [Fact]
        public static void Day18_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day18\input.txt", "100" };
            var target = new Day18();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(814, target.LightsIlluminated);
        }
    }
}
