// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day12"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day12Tests
    {
        [Fact]
        public static void Y2017_Day12_GetProgramsInGroup_Returns_Correct_Value()
        {
            // Arrange
            int programId = 0;

            var pipes = new[]
            {
                "0 <-> 2",
                "1 <-> 1",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4",
                "4 <-> 2, 3, 6",
                "5 <-> 6",
                "6 <-> 4, 5",
            };

            // Arrange
            int actual = Day12.GetProgramsInGroup(programId, pipes);

            // Assert
            Assert.Equal(6, actual);
        }

        [Fact]
        public static void Y2017_Day12_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day12>();

            // Assert
            Assert.Equal(113, puzzle.ProgramsInGroupOfProgram0);
        }
    }
}
