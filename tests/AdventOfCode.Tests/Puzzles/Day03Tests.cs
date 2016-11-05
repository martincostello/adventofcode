// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day03Tests
    {
        [Theory]
        [InlineData(">", 1)]
        [InlineData("^>v<", 4)]
        [InlineData("^v^v^v^v^v", 2)]
        public static void Day03_GetUniqueHousesVisitedBySanta(string instructions, int expected)
        {
            // Act
            int actual = Day03.GetUniqueHousesVisitedBySanta(instructions);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("^v", 3)]
        [InlineData("^>v<", 3)]
        [InlineData("^v^v^v^v^v", 11)]
        public static void Day03_GetUniqueHousesVisitedBySantaAndRoboSanta(string instructions, int expected)
        {
            // Act
            int actual = Day03.GetUniqueHousesVisitedBySantaAndRoboSanta(instructions);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day03_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day03>();

            // Assert
            Assert.Equal(2565, puzzle.HousesWithPresentsFromSanta);
            Assert.Equal(2639, puzzle.HousesWithPresentsFromSantaAndRoboSanta);
        }
    }
}
