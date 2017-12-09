// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day07Tests
    {
        [Fact]
        public static void Y2017_Day07_FindBottomProgramName_Returns_Correct_Value()
        {
            // Arrange
            var structure = new[]
            {
                "pbga (66)",
                "xhth (57)",
                "ebii (61)",
                "havc (66)",
                "ktlj (57)",
                "fwft (72) -> ktlj, cntj, xhth",
                "qoyq (66)",
                "padx (45) -> pbga, havc, qoyq",
                "tknk (41) -> ugml, padx, fwft",
                "jptl (61)",
                "ugml (68) -> gyxo, ebii, jptl",
                "gyxo (61)",
                "cntj (57)",
            };

            // Act
            string actual = Day07.FindBottomProgramName(structure);

            // Assert
            Assert.Equal("tknk", actual);
        }

        [Fact]
        public static void Y2017_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day07>();

            // Assert
            Assert.Equal("fbgguv", puzzle.BottomProgramName);
        }
    }
}
