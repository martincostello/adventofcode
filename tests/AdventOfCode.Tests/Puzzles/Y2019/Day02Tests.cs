// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Collections.Generic;
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
        [InlineData("1,9,10,3,2,3,11,0,99,30,40,50", new[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 })]
        [InlineData("1,0,0,0,99", new[] { 2, 0, 0, 0, 99 })]
        [InlineData("2,3,0,3,99", new[] { 2, 3, 0, 6, 99 })]
        [InlineData("2,4,4,5,99,0", new[] { 2, 4, 4, 5, 99, 9801 })]
        [InlineData("1,1,1,4,99,5,6,0,99", new[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void Y2019_Day02_RunProgram_Returns_Correct_State(string program, int[] expected)
        {
            // Act
            IList<int> actual = Day02.RunProgram(program);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2019_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day02>();

            // Assert
            Assert.NotNull(puzzle.Output);
            Assert.NotEmpty(puzzle.Output);
            Assert.Equal(9581917, puzzle.Output[0]);
        }
    }
}
