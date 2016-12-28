// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day25"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day25Tests
    {
        [Fact]
        public static void Y2016_Day25_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day25>();

            // Assert
            Assert.Equal(198, puzzle.ClockSignalValue);
        }
    }
}
