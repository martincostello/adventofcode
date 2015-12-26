// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day23"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day23Tests
    {
        [Fact]
        public static void Day23_ProcessInstructions()
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
        public static void Day23_Solve_Returns_Correct_Solution_1()
        {
            // Arrange
            string[] args = new string[] { PuzzleTestHelpers.GetInputPath(23) };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day23>(args);

            // Assert
            Assert.Equal(1u, puzzle.A);
            Assert.Equal(170u, puzzle.B);
        }

        [Fact]
        public static void Day23_Solve_Returns_Correct_Solution_2()
        {
            // Arrange
            string[] args = new string[] { PuzzleTestHelpers.GetInputPath(23), "1" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day23>(args);

            // Assert
            Assert.Equal(1u, puzzle.A);
            Assert.Equal(247u, puzzle.B);
        }
    }
}
