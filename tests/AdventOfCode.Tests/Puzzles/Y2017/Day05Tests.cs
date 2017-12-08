// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day05Tests
    {
        [Fact]
        public static void Y2017_Day05_Execute_Returns_Correct_Value()
        {
            // Arrange
            var program = new[] { 0, 3, 0, 1, -3 };

            // Act
            int actual = Day05.Execute(program);

            // Assert
            Assert.Equal(5, actual);
        }

        [Fact]
        public static void Y2017_Day05_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day05>();

            // Assert
            Assert.Equal(373543, puzzle.StepsToExit);
        }
    }
}
