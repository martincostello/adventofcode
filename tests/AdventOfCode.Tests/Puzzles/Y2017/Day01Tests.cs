// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day01Tests
    {
        [Theory]
        [InlineData("1122", 3)]
        [InlineData("1111", 4)]
        [InlineData("1234", 0)]
        [InlineData("91212129", 9)]
        public static void Y2017_Day01_CalculateSum_Returns_Correct_Solution(
            string digits,
            int expected)
        {
            // Act
            int actual = Day01.CalculateSum(digits);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(1034, puzzle.CaptchaSolution);
        }
    }
}
