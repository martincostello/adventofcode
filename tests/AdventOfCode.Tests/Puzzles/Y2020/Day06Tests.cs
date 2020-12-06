// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day06Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day06Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day06Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Y2020_Day06_GetSumOfQuestionsWithYesAnswers_Returns_Correct_Value()
        {
            // Arrange
            string[] values = new[]
            {
                "abc",
                string.Empty,
                "a",
                "b",
                "c",
                string.Empty,
                "ab",
                "ac",
                string.Empty,
                "a",
                "a",
                "a",
                "a",
                string.Empty,
                "b",
            };

            // Act
            int actual = Day06.GetSumOfQuestionsWithYesAnswers(values);

            // Assert
            actual.ShouldBe(11);
        }

        [Fact]
        public void Y2020_Day06_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day06>();

            // Assert
            puzzle.SumOfQuestions.ShouldBe(6542);
        }
    }
}
