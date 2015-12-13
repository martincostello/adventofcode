// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
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
        public static void Day01_GetFinalFloorAndFirstInstructionBasementReached(string value, int floor, int instruction)
        {
            // Act
            Tuple<int, int> result = Day01.GetFinalFloorAndFirstInstructionBasementReached(value);

            // Assert
            Assert.Equal(floor, result.Item1);
            Assert.Equal(instruction, result.Item2);
        }

        [Fact]
        public static void Day01_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day01\input.txt" };
            Day01 target = new Day01();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(232, target.FinalFloor);
            Assert.Equal(1783, target.FirstBasementInstruction);
        }
    }
}
