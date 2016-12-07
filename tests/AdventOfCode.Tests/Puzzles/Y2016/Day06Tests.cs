// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day06Tests
    {
        [Theory]
        [InlineData(new[] { "eedadn", "drvtee", "eandsr", "raavrd", "atevrs", "tsrnev", "sdttsa", "rasrtv", "nssdts", "ntnada", "svetve", "tesnvt", "vntsnd", "vrdear", "dvrsen", "enarar" }, "easter")]
        public static void Y2016_Day06_DecryptMessage_Returns_Correct_Solution(string[] messages, string expected)
        {
            // Act
            string actual = Day06.DecryptMessage(messages);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day06_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day06>();

            // Assert
            Assert.Equal("qzedlxso", puzzle.ErrorCorrectedMessage);
        }
    }
}
