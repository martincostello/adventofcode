// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

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
        [InlineData(1, 5)]
        [InlineData(2, 10)]
        public static void Y2017_Day05_Execute_Returns_Correct_Value(int version, int expected)
        {
            // Arrange
            int[] program = new[] { 0, 3, 0, 1, -3 };

            // Act
            int actual = Day05.Execute(program, version);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2017_Day05_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day05>();

            // Assert
            puzzle.StepsToExitV1.ShouldBe(373543);
            puzzle.StepsToExitV2.ShouldBe(27502966);
        }
    }
}
