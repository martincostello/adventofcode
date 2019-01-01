// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day19Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day19Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day19Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2015_Day19_GetPossibleMolecules()
        {
            // Arrange
            string molecule = "HOH";
            string[] replacements = new[] { "H => HO", "H => OH", "O => HH" };

            // Act
            ICollection<string> result = Day19.GetPossibleMolecules(molecule, replacements);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal("HHHH", result.ElementAt(0));
            Assert.Equal("HOHO", result.ElementAt(1));
            Assert.Equal("HOOH", result.ElementAt(2));
            Assert.Equal("OHOH", result.ElementAt(3));
        }

        [Fact]
        public static void Y2015_Day19_GetMinimumSteps()
        {
            // Arrange
            string molecule = "HOH";
            string[] replacements = new[] { "e => H", "e => O", "H => HO", "H => OH", "O => HH" };

            // Act
            int actual = Day19.GetMinimumSteps(molecule, replacements);

            // Assert
            Assert.Equal(3, actual);
        }

        [Theory]
        [InlineData("calibrate", 576)]
        [InlineData("fabricate", 207, Skip = "Too slow.")]
        public void Y2015_Day19_Solve_Returns_Correct_Solution(string mode, int expected)
        {
            // Arrange
            string[] args = new[] { mode };

            // Act
            var puzzle = SolvePuzzle<Day19>(args);

            // Assert
            Assert.Equal(expected, puzzle.Solution);
        }
    }
}
