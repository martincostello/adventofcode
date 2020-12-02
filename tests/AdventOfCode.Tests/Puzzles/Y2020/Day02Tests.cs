// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
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
        [InlineData("1-3 a: abcde", true)]
        [InlineData("1-3 b: cdefg", false)]
        [InlineData("2-9 c: ccccccccc", true)]
        public void Y2020_Day02_IsPasswordValid_Returns_Correct_Value(string value, bool expected)
        {
            // Act
            bool actual = Day02.IsPasswordValid(value);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2020_Day02_GetValidPasswordCount_Returns_Correct_Value()
        {
            // Arrange
            string[] values = new[]
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc",
            };

            // Act
            int actual = Day02.GetValidPasswordCount(values);

            // Assert
            actual.ShouldBe(2);
        }

        [Fact]
        public void Y2020_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day02>();

            // Assert
            puzzle.ValidPasswords.ShouldBe(542);
        }
    }
}
