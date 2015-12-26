// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day16Tests
    {
        [Fact]
        public static void Day16_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { PuzzleTestHelpers.GetInputPath(16) };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day16>(args);

            // Assert
            Assert.Equal(373, puzzle.AuntSueNumber);
            Assert.Equal(260, puzzle.RealAuntSueNumber);
        }
    }
}
