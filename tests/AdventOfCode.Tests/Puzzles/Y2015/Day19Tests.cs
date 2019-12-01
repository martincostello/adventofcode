// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Collections.Generic;
    using Shouldly;
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
            ICollection<string> actual = Day19.GetPossibleMolecules(molecule, replacements);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldBe(new[] { "HHHH", "HOHO", "HOOH", "OHOH" });
        }

        [Fact]
        public void Y2015_Day19_GetMinimumSteps()
        {
            // Arrange
            string molecule = "HOH";
            string[] replacements = new[] { "e => H", "e => O", "H => HO", "H => OH", "O => HH" };

            // Act
            int actual = Day19.GetMinimumSteps(molecule, replacements, Logger);

            // Assert
            actual.ShouldBe(3);
        }

        [Theory]
        [InlineData(new[] { "calibrate" }, 576)]
        [InlineData(new[] { "fabricate" }, 207, Skip = "Too slow.")]
        public void Y2015_Day19_Solve_Returns_Correct_Solution(string[] args, int expected)
        {
            // Act
            var puzzle = SolvePuzzle<Day19>(args);

            // Assert
            puzzle.Solution.ShouldBe(expected);
        }
    }
}
