﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class containing tests for the <see cref="Day23"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day23Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day23Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day23Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(new[] { "7" }, 14346)]
        [InlineData(new[] { "12" }, 479010906, Skip = "Too slow.")]
        public async Task Y2016_Day23_Solve_Returns_Correct_Solution(string[] args, int expected)
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day23>(args);

            // Assert
            puzzle.SafeValue.ShouldBe(expected);
        }
    }
}
