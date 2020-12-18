// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day18"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day18Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day18Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day18Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("11", 11)]
        [InlineData("5 * 3", 15)]
        [InlineData("(5 * 3)", 15)]
        [InlineData("((5 * 3))", 15)]
        [InlineData("11 + 3", 14)]
        [InlineData("(11 + 3)", 14)]
        [InlineData("((11 + 3))", 14)]
        [InlineData("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Y2020_Day18_Evaluate_Returns_Correct_Value(string expression, long expected)
        {
            // Act
            long actual = Day18.Evaluate(new[] { expression });

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2020_Day18_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day18>();

            // Assert
            puzzle.Sum.ShouldBe(2743012121210L);
        }
    }
}
