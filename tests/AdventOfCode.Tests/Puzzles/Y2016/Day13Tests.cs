// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day13"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day13Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day13Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day13Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(10, 7, 4, 11)]
        public static void Y2016_Day13_GetMinimumStepsToReachCoordinate_Returns_Correct_Solution(
            int favoriteNumber,
            int x,
            int y,
            int expected)
        {
            // Act
            int actual = Day13.GetMinimumStepsToReachCoordinate(favoriteNumber, x, y);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day13_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day13>("1362");

            // Assert
            puzzle.FewestStepsToReach31X39Y.ShouldBe(82);
            puzzle.LocationsWithin50.ShouldBe(138);
        }
    }
}
