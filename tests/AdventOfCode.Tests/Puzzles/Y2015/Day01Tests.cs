// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day01Tests
    {
        [Theory]
        [InlineData("(())", 0, -1)]
        [InlineData("()()", 0, -1)]
        [InlineData("(((", 3, -1)]
        [InlineData("(()(()(", 3, -1)]
        [InlineData("))(((((", 3, 1)]
        [InlineData("())", -1, 3)]
        [InlineData("))(", -1, 1)]
        [InlineData(")))", -3, 1)]
        [InlineData(")())())", -3, 1)]
        [InlineData(")", -1, 1)]
        [InlineData("()())", -1, 5)]
        public static void Y2015_Day01_GetFinalFloorAndFirstInstructionBasementReached(string value, int floor, int instruction)
        {
            // Act
            Tuple<int, int> result = Day01.GetFinalFloorAndFirstInstructionBasementReached(value);

            // Assert
            Assert.Equal(floor, result.Item1);
            Assert.Equal(instruction, result.Item2);
        }

        [Fact]
        public static void Y2015_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(232, puzzle.FinalFloor);
            Assert.Equal(1783, puzzle.FirstBasementInstruction);
        }
    }
}
