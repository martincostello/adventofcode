// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day04Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day04Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day04Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("abcdef", 5, 609043)]
        [InlineData("pqrstuv", 5, 1048970)]
        public static void Y2015_Day04_GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes, int expected)
        {
            // Act
            int actual = Day04.GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { "iwrupvqb", "5" }, 346386)]
        [InlineData(new[] { "iwrupvqb", "6" }, 9958218)]
        public void Y2015_Day04_Solve_Returns_Correct_Solution(string[] args, int expected)
        {
            // Act
            var target = SolvePuzzle<Day04>(args);

            // Assert
            target.LowestZeroHash.ShouldBe(expected);
        }
    }
}
