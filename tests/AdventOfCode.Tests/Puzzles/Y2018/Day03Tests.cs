﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day03Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day03Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day03Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(new[] { "#1 @ 1,3: 4x4", "#2 @ 3,1: 4x4", "#3 @ 5,5: 2x2" }, 4)]
        public static void Y2018_Day03_GetAreaWithTwoOrMoreOverlappingClaims_Returns_Correct_Solution(
            string[] sequence,
            int expected)
        {
            // Act
            int actual = Day03.GetAreaWithTwoOrMoreOverlappingClaims(sequence);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2018_Day03_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day03>();

            // Assert
            Assert.Equal(100595, puzzle.Area);
        }
    }
}
