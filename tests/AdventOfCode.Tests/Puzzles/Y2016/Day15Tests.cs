// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day15"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day15Tests
    {
        [Fact]
        public static void Y2016_Day15_FindTimeForCapsuleRelease_Returns_Correct_Solution()
        {
            // Arrange
            string[] input = new[]
            {
                "Disc #1 has 5 positions; at time=0, it is at position 4.",
                "Disc #2 has 2 positions; at time=0, it is at position 1.",
            };

            // Act
            int actual = Day15.FindTimeForCapsuleRelease(input);

            // Assert
            Assert.Equal(5, actual);
        }

        [Fact]
        public static void Y2016_Day15_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day15>();

            // Assert
            Assert.Equal(16824, puzzle.TimeOfFirstButtonPress);
        }
    }
}
