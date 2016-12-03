// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day01Tests
    {
        [Theory]
        [InlineData("R2, L3", true, 5)]
        [InlineData("R2, R2, R2", true, 2)]
        [InlineData("R5, L5, R5, R3", true, 12)]
        [InlineData("R8, R4, R4, R8", false, 4)]
        public static void Y2016_Day01_CalculateDistance_Returns_Correct_Solution(
            string instructions,
            bool ignoreDuplicates,
            int expected)
        {
            // Act
            int actual = Day01.CalculateDistance(instructions, ignoreDuplicates);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(287, puzzle.BlockToEasterBunnyHQ);
        }

        [Fact]
        public static void Y2016_Day01_Solve_Returns_Correct_Solution_With_Duplicates()
        {
            // Arrange
            string[] args = new[] { "false" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day01>(args);

            // Assert
            Assert.Equal(133, puzzle.BlockToEasterBunnyHQ);
        }
    }
}
