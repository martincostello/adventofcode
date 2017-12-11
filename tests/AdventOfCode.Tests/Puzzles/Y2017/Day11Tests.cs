// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day11Tests
    {
        [Theory]
        [InlineData("ne,ne,ne", 3)]
        [InlineData("ne,ne,sw,sw", 0)]
        [InlineData("ne,ne,s,s", 2)]
        [InlineData("se,sw,se,sw,sw", 3)]
        public static void Y2017_Day11_FindMinimumSteps_Returns_Correct_Solution(string path, int expected)
        {
            // Arrange
            int actual = Day11.FindMinimumSteps(path);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day11_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day11>();

            // Assert
            Assert.Equal(796, puzzle.MinimumSteps);
        }
    }
}
