// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day15"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day15Tests
    {
        [Fact]
        public static void Y2015_Day15_GetHighestTotalCookieScore()
        {
            // Arrange
            string[] collection = new[]
            {
                "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
                "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3",
            };

            // Act
            int actual = Day15.GetHighestTotalCookieScore(collection);

            // Assert
            Assert.Equal(62842880, actual);

            // Act
            actual = Day15.GetHighestTotalCookieScore(collection, 500);

            // Assert
            Assert.Equal(57600000, actual);
        }

        [Fact]
        public static void Y2015_Day15_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day15>();

            // Assert
            Assert.Equal(222870, puzzle.HighestTotalCookieScore);
            Assert.Equal(117936, puzzle.HighestTotalCookieScoreWith500Calories);
        }
    }
}
