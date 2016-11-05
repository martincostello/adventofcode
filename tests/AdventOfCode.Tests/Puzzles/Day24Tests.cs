// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day24"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day24Tests
    {
        [Fact]
        public static void Day24_GetQuantumEntanglementOfIdealConfiguration()
        {
            // Arrange
            int compartments = 3;
            int[] weights = new[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11 };

            // Act
            long actual = Day24.GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

            // Assert
            Assert.Equal(99, actual);

            // Arrange
            compartments = 4;

            // Act
            actual = Day24.GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

            // Assert
            Assert.Equal(44, actual);
        }

        [Fact(Skip = "Too slow.")]
        public static void Day24_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day24>();

            // Assert
            Assert.Equal(11266889531, puzzle.QuantumEntanglementOfFirstGroup);

            // Arrange
            string[] args = new string[] { "4" };

            // Act
            puzzle = PuzzleTestHelpers.SolvePuzzle<Day24>(args);

            // Assert
            Assert.Equal(77387711, puzzle.QuantumEntanglementOfFirstGroup);
        }
    }
}
