// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day06Tests
    {
        [Fact]
        public static void Y2017_Day06_Debug_Returns_Correct_Value()
        {
            // Arrange
            var memory = new[] { 0, 2, 7, 0 };

            // Act
            int actual = Day06.Debug(memory);

            // Assert
            Assert.Equal(5, actual);
        }

        [Fact]
        public static void Y2017_Day06_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day06>();

            // Assert
            Assert.Equal(3156, puzzle.CycleCount);
        }
    }
}
