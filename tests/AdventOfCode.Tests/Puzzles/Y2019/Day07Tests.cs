// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day07Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day07Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day07Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", 43210)]
        [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", 54321)]
        [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", 65210)]
        public void Y2019_Day07_RunProgram_Returns_Correct_Output(string program, long expected)
        {
            // Act
            long actual = Day07.RunProgram(program);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2019_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day07>();

            // Assert
            puzzle.HighestSignal.ShouldBe(77500);
        }
    }
}
