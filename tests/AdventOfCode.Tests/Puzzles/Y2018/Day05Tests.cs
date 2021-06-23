// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    /// <summary>
    /// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day05Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day05Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day05Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("aA", "")]
        [InlineData("Aa", "")]
        [InlineData("baA", "b")]
        [InlineData("abBA", "")]
        [InlineData("abAB", "abAB")]
        [InlineData("aabAAB", "aabAAB")]
        [InlineData("dabAcCaCBAcCcaDA", "dabCBAcaDA")]
        public static void Y2018_Day05_Reduce_Returns_Correct_Solution(
            string polymer,
            string expected)
        {
            // Act
            string actual = Day05.Reduce(polymer);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("dabAcCaCBAcCcaDA", "daDA")]
        public static void Y2018_Day05_ReduceWithOptimization_Returns_Correct_Solution(
            string polymer,
            string expected)
        {
            // Act
            string actual = Day05.ReduceWithOptimization(polymer);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2018_Day05_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day05>();

            // Assert
            puzzle.RemainingUnits.ShouldBe(10638);
            puzzle.RemainingUnitsOptimized.ShouldBe(4944);
        }
    }
}
