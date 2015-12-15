// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day15"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day15Tests
    {
        [Fact]
        public static void Day15_GetHighestTotalCookieScore()
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
        }

        [Fact(Skip = "Not implemented fully yet.")]
        public static void Day15_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day15\input.txt" };
            var target = new Day15();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.NotEqual(0, target.HighestTotalCookieScore);
        }
    }
}
