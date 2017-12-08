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
        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 10)]
        public static void Y2017_Day05_Execute_Returns_Correct_Value(int version, int expected)
        {
            // Arrange
            var program = new[] { 0, 3, 0, 1, -3 };

            // Act
            int actual = Day05.Execute(program, version);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day05_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day05>();

            // Assert
            Assert.Equal(373543, puzzle.StepsToExitV1);
            Assert.Equal(27502966, puzzle.StepsToExitV2);
        }
    }
}
