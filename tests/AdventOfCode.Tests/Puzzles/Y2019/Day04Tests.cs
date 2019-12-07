// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
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
        [InlineData("111111", true)]
        [InlineData("111123", true)]
        [InlineData("122345", true)]
        [InlineData("135679", false)]
        [InlineData("123789", false)]
        [InlineData("223450", false)]
        public void Y2019_Day04_Is_Valid_Returns_Correct_Value(string password, bool expected)
        {
            // Act
            bool actual = Day04.IsValid(password);

            // Assert
            expected.ShouldBe(actual);
        }

        [Fact]
        public void Y2019_Day04_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "138241-674034" };

            // Act
            var puzzle = SolvePuzzle<Day04>(args);

            // Assert
            puzzle.Count.ShouldBe(1890);
        }
    }
}
