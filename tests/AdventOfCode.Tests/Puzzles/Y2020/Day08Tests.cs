﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    /// <summary>
    /// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day08Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day08Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day08Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(false, 5)]
        [InlineData(true, 8)]
        public void Y2020_Day08_RunProgram_Returns_Correct_Value(bool fix, int expected)
        {
            // Arrange
            string[] values = new[]
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6",
            };

            // Act
            int actual = Day08.RunProgram(values, fix);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2020_Day08_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day08>();

            // Assert
            puzzle.Accumulator.ShouldBe(1137);
            puzzle.AccumulatorWithFix.ShouldBe(1125);
        }
    }
}
