// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Collections.Generic;
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

        [Fact]
        public static void Y2015_Day07_InstructionsAreInterpretedCorrectly()
        {
            // Arrange
            string[] instructions = new string[]
            {
                "j -> k",
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i",
                "i -> j",
            };

            // Act
            IDictionary<string, ushort> actual = Day07.GetWireValues(instructions);

            // Assert
            Assert.Equal(72, actual["d"]);
            Assert.Equal(507, actual["e"]);
            Assert.Equal(492, actual["f"]);
            Assert.Equal(114, actual["g"]);
            Assert.Equal(65412, actual["h"]);
            Assert.Equal(65079, actual["i"]);
            Assert.Equal(65079, actual["j"]);
            Assert.Equal(65079, actual["k"]);
            Assert.Equal(123, actual["x"]);
            Assert.Equal(456, actual["y"]);
        }

        [Fact]
        public void Y2015_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day07>();

            // Assert
            Assert.Equal(3176, puzzle.FirstSignal);
            Assert.Equal(14710, puzzle.SecondSignal);
        }
    }
}
