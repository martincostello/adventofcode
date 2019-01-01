// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day12"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day12Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day12Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day12Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2016_Day12_Process_Returns_Correct_Solution()
        {
            // Arrange
            string[] instructions = new[]
            {
                "cpy 41 a",
                "inc a",
                "inc a",
                "dec a",
                "jnz a 2",
                "dec a",
            };

            int[] binsOfInterest = new[] { 0, 1, 2 };

            // Act
            var actual = Day12.Process(instructions, initialValueOfC: 0);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(4, actual.Count);
            Assert.Equal(42, actual['a']);
            Assert.Equal(0, actual['b']);
            Assert.Equal(0, actual['c']);
            Assert.Equal(0, actual['d']);
        }

        [Fact]
        public static void Y2016_Day12_Process_With_Toggle_Returns_Correct_Solution()
        {
            // Arrange
            string[] instructions = new[]
            {
                "cpy 2 a",
                "tgl a",
                "tgl a",
                "tgl a",
                "cpy 1 a",
                "dec a",
                "dec a",
            };

            int[] binsOfInterest = new[] { 0, 1, 2 };

            // Act
            var actual = Day12.Process(instructions, initialValueOfC: 0);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(4, actual.Count);
            Assert.Equal(3, actual['a']);
            Assert.Equal(0, actual['b']);
            Assert.Equal(0, actual['c']);
            Assert.Equal(0, actual['d']);
        }

        [Fact]
        public void Y2016_Day12_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day12>();

            // Assert
            Assert.Equal(318020, puzzle.ValueInRegisterA);
            Assert.Equal(9227674, puzzle.ValueInRegisterAWhenInitializedWithIgnitionKey);
        }
    }
}
