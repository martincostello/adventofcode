// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day16Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day16Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day16Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("110010110100", 12, "100")]
        [InlineData("10000", 20, "01100")]
        public static void Y2016_Day16_GetDiskChecksum_Returns_Correct_Solution(string initial, int size, string expected)
        {
            // Act
            string actual = Day16.GetDiskChecksum(initial, size);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("272", "10010110010011110")]
        [InlineData("35651584", "01101011101100011")]
        public void Y2016_Day16_Solve_Returns_Correct_Solution(string size, string expected)
        {
            // Arrange
            string[] args = new[] { "10010000000110000", size };

            // Act
            var puzzle = SolvePuzzle<Day16>(args);

            // Assert
            puzzle.Checksum.ShouldBe(expected);
        }
    }
}
