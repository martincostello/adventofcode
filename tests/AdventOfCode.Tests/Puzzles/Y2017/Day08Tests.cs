// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day08Tests
    {
        [Fact]
        public static void Y2017_Day08_FindHighestRegisterValueAtEnd_Returns_Correct_Value()
        {
            // Arrange
            var instructions = new[]
            {
                "b inc 5 if a > 1",
                "a inc 1 if b < 5",
                "c dec -10 if a >= 1",
                "c inc -20 if c == 10",
            };

            // Act
            int actual = Day08.FindHighestRegisterValueAtEnd(instructions);

            // Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public static void Y2017_Day08_FindHighestRegisterValueDuring_Returns_Correct_Value()
        {
            // Arrange
            var instructions = new[]
            {
                "b inc 5 if a > 1",
                "a inc 1 if b < 5",
                "c dec -10 if a >= 1",
                "c inc -20 if c == 10",
            };

            // Act
            int actual = Day08.FindHighestRegisterValueDuring(instructions);

            // Assert
            Assert.Equal(10, actual);
        }

        [Fact]
        public static void Y2017_Day08_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day08>();

            // Assert
            Assert.Equal(7296, puzzle.HighestRegisterValueAtEnd);
            Assert.Equal(8186, puzzle.HighestRegisterValueDuring);
        }
    }
}
