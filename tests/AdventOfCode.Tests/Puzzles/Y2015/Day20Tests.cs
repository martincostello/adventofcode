// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day20"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day20Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day20Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day20Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

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
        public static void Y2015_Day20_GetPresentsDelivered(int house, int expected, int? maximumVisits)
        {
            // Act
            int actual = Day20.GetPresentsDelivered(house, maximumVisits);

            // Assert
            actual.ShouldBe(expected);
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
        public static void Y2015_Day20_GetLowestHouseNumber(int target, int expected)
        {
            // Arrange
            int? maximumVisits = null;

            // Act
            int actual = Day20.GetLowestHouseNumber(target, maximumVisits);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { "34000000" }, 786240)]
        [InlineData(new[] { "34000000", "50" }, 831600)]
        public void Y2015_Day20_Solve_Returns_Correct_Solution(string[] args, int expected)
        {
            // Act
            var puzzle = SolvePuzzle<Day20>(args);

            // Assert
            puzzle.LowestHouseNumber.ShouldBe(expected);
        }
    }
}
