// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day23"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day23Tests
    {
        [Theory]
        [InlineData("7", 14346)]
        [InlineData("12", 479010906, Skip = "Too slow.")]
        public static void Y2016_Day23_Solve_Returns_Correct_Solution(string input, int expected)
        {
            // Arrange
            string[] args = new[] { input };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day23>(args);

            // Assert
            Assert.Equal(expected, puzzle.SafeValue);
        }
    }
}
