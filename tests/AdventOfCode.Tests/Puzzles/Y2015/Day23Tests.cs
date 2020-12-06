// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

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

        [Fact]
        public void Y2015_Day23_ProcessInstructions()
        {
            // Arrange
            IList<string> instructions = new[]
            {
                "inc a",
                "jio a, +2",
                "tpl a",
                "inc a",
            };

            uint initialValue = 0;

            // Act
            (uint a, uint b) = Day23.ProcessInstructions(instructions, initialValue, Logger);

            // Assert
            a.ShouldBe(2u);
            b.ShouldBe(0u);
        }

        [Theory]
        [InlineData(new string[0], 1u, 170u)]
        [InlineData(new[] { "1" }, 1u, 247u)]
        public async Task Y2015_Day23_Solve_Returns_Correct_Solution(string[] args, uint a, uint b)
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day23>(args);

            // Assert
            puzzle.A.ShouldBe(a);
            puzzle.B.ShouldBe(b);
        }
    }
}
