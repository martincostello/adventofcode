// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day02Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day02Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day02Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(new[] { 5, 1, 9, 5 }, 8)]
        [InlineData(new[] { 7, 5, 3 }, 4)]
        [InlineData(new[] { 2, 4, 6, 8 }, 6)]
        public static void Y2017_Day02_ComputeDifference_Returns_Correct_Solution(int[] row, int expected)
        {
            // Act
            int actual = Day02.ComputeDifference(row);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { 5, 9, 2, 8 }, 4)]
        [InlineData(new[] { 9, 4, 7, 3 }, 3)]
        [InlineData(new[] { 3, 8, 6, 5 }, 2)]
        public static void Y2017_Day02_ComputeDivisionOfEvenlyDivisible_Returns_Correct_Solution(int[] row, int expected)
        {
            // Act
            int actual = Day02.ComputeDivisionOfEvenlyDivisible(row);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public static void Y2017_Day02_CalculateChecksum_Returns_Correct_Solution_Using_Difference()
        {
            // Arrange
            int[][] spreadsheet = new[]
            {
                new[] { 5, 1, 9, 5 },
                new[] { 7, 5, 3 },
                new[] { 2, 4, 6, 8 },
            };

            // Act
            int actual = Day02.CalculateChecksum(spreadsheet, forEvenlyDivisible: false);

            // Assert
            actual.ShouldBe(18);
        }

        [Fact]
        public static void Y2017_Day02_CalculateChecksum_Returns_Correct_Solution_Using_Evenly_Divisible()
        {
            // Arrange
            int[][] spreadsheet = new[]
            {
                new[] { 5, 9, 2, 8 },
                new[] { 9, 4, 7, 3 },
                new[] { 3, 8, 6, 5 },
            };

            // Act
            int actual = Day02.CalculateChecksum(spreadsheet, forEvenlyDivisible: true);

            // Assert
            actual.ShouldBe(9);
        }

        [Fact]
        public async Task Y2017_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day02>();

            // Assert
            puzzle.ChecksumForDifference.ShouldBe(36174);
            puzzle.ChecksumForEvenlyDivisible.ShouldBe(244);
        }
    }
}
