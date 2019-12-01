// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day18"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day18Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day18Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day18Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("..^^.", 3, 6)]
        [InlineData(".^^.^.^^^^", 10, 38)]
        public void Y2016_Day18_FindSafeTileCount_Returns_Correct_Solution(string firstRowTiles, int rows, int expected)
        {
            // Act
            int actual = Day18.FindSafeTileCount(firstRowTiles, rows, Logger);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("40", 1987)]
        [InlineData("400000", 19984714)]
        public void Y2016_Day18_Solve_Returns_Correct_Solution(string rows, int expected)
        {
            // Arrange
            string[] args = new[] { rows };

            // Act
            var puzzle = SolvePuzzle<Day18>(args);

            // Assert
            puzzle.SafeTileCount.ShouldBe(expected);
        }
    }
}
