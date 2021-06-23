// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
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
        [InlineData(new[] { 1, -2, 3, +1 }, 3)]
        [InlineData(new[] { 1, 1, 1 }, 3)]
        [InlineData(new[] { 1, 1, -2 }, 0)]
        [InlineData(new[] { -1, -2, -3 }, -6)]
        public static void Y2018_Day01_CalculateFrequency_Returns_Correct_Solution(
            int[] sequence,
            int expected)
        {
            // Act
            int actual = Day01.CalculateFrequency(sequence);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { 1, -2, 3, 1, 1, -2 }, 2)]
        [InlineData(new[] { 1, -1 }, 0)]
        [InlineData(new[] { 3, 3, 4, -2, -4 }, 10)]
        [InlineData(new[] { -6, 3, 8, 5, -6 }, 5)]
        [InlineData(new[] { 7, 7, -2, -7, -4 }, 14)]
        public static void Y2018_Day01_CalculateFrequencyWithRepetition_Returns_Correct_Solution(
            int[] sequence,
            int expected)
        {
            // Act
            int actual = Day01.CalculateFrequencyWithRepetition(sequence).firstRepeat;

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2018_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day01>();

            // Assert
            puzzle.Frequency.ShouldBe(543);
            puzzle.FirstRepeatedFrequency.ShouldBe(621);
        }
    }
}
