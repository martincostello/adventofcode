﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
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
        [InlineData(new[] { "rect 3x2", "rotate column x=1 by 1", "rotate row y=0 by 4", "rotate column x=1 by 1" }, 7, 3, 6)]
        public void Y2016_Day08_GetPixelsLit_Returns_Correct_Solution(string[] instructions, int width, int height, int expected)
        {
            // Act
            (int actual, _) = Day08.GetPixelsLit(instructions, width, height, Logger);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day08_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day08>();

            // Assert
            puzzle.PixelsLit.ShouldBe(121);
        }
    }
}
