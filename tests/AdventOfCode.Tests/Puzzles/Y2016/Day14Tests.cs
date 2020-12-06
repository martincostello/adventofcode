// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day14"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day14Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day14Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day14Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("abc", 64, false, 22728)]
        [InlineData("abc", 64, true, 22551, Skip = "Too slow.")]
        public static void Y2016_Day14_GetOneTimePadKeyIndex_Returns_Correct_Solution(
            string salt,
            int ordinal,
            bool useKeyStretching,
            int expected)
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));

            // Act
            int actual = Day14.GetOneTimePadKeyIndex(salt, ordinal, useKeyStretching, cts.Token);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact(Skip = "Too slow.")]
        public async Task Y2016_Day14_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "ihaygndm" };

            // Act
            var puzzle = await SolvePuzzleAsync<Day14>(args);

            // Assert
            puzzle.IndexOfKey64.ShouldBe(15035);
            puzzle.IndexOfKey64WithStretching.ShouldBe(19968);
        }
    }
}
