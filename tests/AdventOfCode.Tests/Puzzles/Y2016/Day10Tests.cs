// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day10"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day10Tests
    {
        [Fact]
        public static void Y2016_Day10_GetBotNumber_Returns_Correct_Solution()
        {
            // Arrange
            string[] instructions = new[]
            {
                "value 5 goes to bot 2",
                "bot 2 gives low to bot 1 and high to bot 0",
                "value 3 goes to bot 1",
                "bot 1 gives low to output 1 and high to bot 0",
                "bot 0 gives low to output 2 and high to output 0",
                "value 2 goes to bot 2",
            };

            int[] binsOfInterest = new[] { 0, 1, 2 };

            // Act
            var actual = Day10.GetBotNumber(instructions, 5, 2, binsOfInterest);

            // Assert
            Assert.Equal(2, actual.Item1);
            Assert.Equal(30, actual.Item2);
        }

        [Fact]
        public static void Y2016_Day10_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day10>();

            // Assert
            Assert.Equal(141, puzzle.BotThatCompares61And17Microchips);
            Assert.Equal(1209, puzzle.ProductOfMicrochipsInBins012);
        }
    }
}
