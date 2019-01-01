// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day01Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day01Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day01Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("1122", false, 3)]
        [InlineData("1111", false, 4)]
        [InlineData("1234", false, 0)]
        [InlineData("91212129", false, 9)]
        [InlineData("1212", true, 6)]
        [InlineData("1221", true, 0)]
        [InlineData("123425", true, 4)]
        [InlineData("123123", true, 12)]
        [InlineData("12131415", true, 4)]
        public static void Y2017_Day01_CalculateSum_Returns_Correct_Solution(
            string digits,
            bool nextDigit,
            int expected)
        {
            // Act
            int actual = Day01.CalculateSum(digits, nextDigit);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2017_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(1034, puzzle.CaptchaSolutionNext);
            Assert.Equal(1356, puzzle.CaptchaSolutionOpposite);
        }
    }
}
