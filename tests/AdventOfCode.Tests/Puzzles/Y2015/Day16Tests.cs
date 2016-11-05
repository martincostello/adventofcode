// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day16Tests
    {
        [Fact]
        public static void Y2015_Day16_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day16>();

            // Assert
            Assert.Equal(373, puzzle.AuntSueNumber);
            Assert.Equal(260, puzzle.RealAuntSueNumber);
        }
    }
}
