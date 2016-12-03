// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day03Tests
    {
        [Theory]
        [InlineData(5, 10, 25, false)]
        public static void Y2016_Day03_IsTriangleValid_Returns_Correct_Solution(int a, int b, int c, bool expected)
        {
            // Act
            bool actual = Day03.IsValidTriangle(a, b, c);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day03_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day03>();

            // Assert
            Assert.Equal(983, puzzle.PossibleTriangles);
        }
    }
}
