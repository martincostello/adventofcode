// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
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
        [InlineData("3,0,4,0,99", 0, 0)]
        [InlineData("3,0,4,0,99", 1, 1)]
        [InlineData("3,0,4,0,99", 2, 2)]
        [InlineData("1002,4,3,4,33", 0, 0)]
        [InlineData("1101,100,-1,4,0", 0, 0)]
        public void Y2019_Day05_RunProgram_Returns_Correct_State(string program, int input, int expected)
        {
            // Act
            int actual = Day05.RunProgram(program, input);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2019_Day05_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day05>();

            // Assert
            puzzle.DiagnosticCode.ShouldBe(6745903);
        }
    }
}
