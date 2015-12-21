// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day20"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day20Tests
    {
        [Theory]
        [InlineData(1, 10, null)]
        [InlineData(2, 30, null)]
        [InlineData(3, 40, null)]
        [InlineData(4, 70, null)]
        [InlineData(5, 60, null)]
        [InlineData(6, 120, null)]
        [InlineData(7, 80, null)]
        [InlineData(8, 150, null)]
        [InlineData(9, 130, null)]
        [InlineData(1, 11, 2)]
        [InlineData(2, 33, 2)]
        [InlineData(3, 33, 2)]
        [InlineData(4, 66, 2)]
        [InlineData(5, 55, 2)]
        [InlineData(6, 99, 2)]
        public static void Day20_GetPresentsDelivered(int house, int expected, int? maximumVisits)
        {
            // Act
            int actual = Day20.GetPresentsDelivered(house, maximumVisits);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData(30, 2)]
        [InlineData(40, 3)]
        [InlineData(70, 4)]
        [InlineData(60, 4)]
        [InlineData(120, 6)]
        [InlineData(80, 6)]
        [InlineData(150, 8)]
        [InlineData(130, 8)]
        public static void Day20_GetLowestHouseNumber(int target, int expected)
        {
            // Arrange
            int? maximumVisits = null;

            // Act
            int actual = Day20.GetLowestHouseNumber(target, maximumVisits);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact(Skip = "Too slow.")]
        public static void Day20_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "34000000" };
            var target = new Day20();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(786240, target.LowestHouseNumber);
        }

        [Fact(Skip = "Too slow.")]
        public static void Day20_Solve_Returns_Correct_Solution_Maximum_Visits()
        {
            // Arrange
            string[] args = new[] { "34000000", "50" };
            var target = new Day20();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(0, target.LowestHouseNumber);
        }
    }
}
