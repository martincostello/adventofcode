// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day11Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day11Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day11Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2016_Day11_GetMinimumStepsForAssembly_Returns_Correct_Solution()
        {
            // Arrange
            string[] initialState = new[]
            {
                "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.",
                "The second floor contains a hydrogen generator.",
                "The third floor contains a lithium generator.",
                "The fourth floor contains nothing relevant.",
            };

            // Act
            int actual = Day11.GetMinimumStepsForAssembly(initialState, useExtra: false);

            // Assert
            actual.ShouldBe(11);
        }

        [Fact]
        public async Task Y2016_Day11_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day11>();

            // Assert
            puzzle.MinimumStepsForAssembly.ShouldBe(47);
            puzzle.MinimumStepsForAssemblyWithExtraParts.ShouldBe(71);
        }
    }
}
