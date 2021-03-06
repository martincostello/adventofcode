﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    /// <summary>
    /// A class containing tests for the <see cref="Day22"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day22Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day22Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day22Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(false, 306)]
        [InlineData(true, 291)]
        public void Y2020_Day22_PlayCombat_Returns_Correct_Value(bool recursive, int expected)
        {
            // Arrange
            string[] values = new[]
            {
                "Player 1:",
                "9",
                "2",
                "6",
                "3",
                "1",
                string.Empty,
                "Player 2:",
                "5",
                "8",
                "4",
                "7",
                "10",
            };

            // Act
            int actual = Day22.PlayCombat(values, recursive);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2020_Day22_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day22>();

            // Assert
            puzzle.WinningScore.ShouldBe(33694);
            puzzle.WinningScoreRecursive.ShouldBe(31835);
        }
    }
}
