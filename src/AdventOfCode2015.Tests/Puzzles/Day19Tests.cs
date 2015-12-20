// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day19Tests
    {
        [Fact]
        public static void Day19_GetPossibleMolecules()
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
        public static void Day19_GetMinimumSteps()
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
        ////[InlineData("fabricate", 0)]
        public static void Day19_Solve_Returns_Correct_Solution(string mode, int expected)
        {
            // Arrange
            string[] args = new[] { @".\Input\Day19\input.txt", mode };
            var target = new Day19();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(expected, target.Solution);
        }
    }
}
