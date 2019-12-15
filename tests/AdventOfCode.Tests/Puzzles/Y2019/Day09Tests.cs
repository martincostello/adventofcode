// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Linq;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day09Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day09Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day09Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", 0, 99, new[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99L })]
        [InlineData("1102,34915192,34915192,7,4,7,99,0", 0, 1219070632396864, null)]
        [InlineData("104,1125899906842624,99", 0, 1125899906842624, null)]
        public void Y2019_Day09_RunProgram_Returns_Correct_Output(
            string program,
            int input,
            long expectedOutput,
            long[] expectedMemory)
        {
            // Act
            (long actualOutput, var actualMemory) = Day09.RunProgram(program, input);

            // Assert
            actualOutput.ShouldBe(expectedOutput);

            if (expectedMemory != null)
            {
                actualMemory.Take(expectedMemory.Length).ToArray().ShouldBe(expectedMemory);
            }
        }

        [Fact]
        public void Y2019_Day09_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day09>();

            // Assert
            puzzle.Keycode.ShouldBe(2494485073);
        }
    }
}
