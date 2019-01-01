// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day23"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day23Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day23Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day23Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2015_Day23_ProcessInstructions()
        {
            // Arrange
            IList<string> instructions = new[]
            {
                "inc a",
                "jio a, +2",
                "tpl a",
                "inc a",
            };

            uint initialValue = 0;

            // Act
            Tuple<uint, uint> actual = Day23.ProcessInstructions(instructions, initialValue);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2u, actual.Item1);
            Assert.Equal(0u, actual.Item2);
        }

        [Fact]
        public void Y2015_Day23_Solve_Returns_Correct_Solution_1()
        {
            // Act
            var puzzle = SolvePuzzle<Day23>();

            // Assert
            Assert.Equal(1u, puzzle.A);
            Assert.Equal(170u, puzzle.B);
        }

        [Fact]
        public void Y2015_Day23_Solve_Returns_Correct_Solution_2()
        {
            // Arrange
            string[] args = new string[] { "1" };

            // Act
            var puzzle = SolvePuzzle<Day23>(args);

            // Assert
            Assert.Equal(1u, puzzle.A);
            Assert.Equal(247u, puzzle.B);
        }
    }
}
