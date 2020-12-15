// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
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

        [Fact]
        public void Y2020_Day14_RunProgram_Returns_Correct_Value_V1()
        {
            // Arrange
            string[] program = new[]
            {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0",
            };

            // Act
            long actual = Day14.RunProgram(program, version: 1);

            // Assert
            actual.ShouldBe(165);
        }

        [Fact]
        public void Y2020_Day14_RunProgram_Returns_Correct_Value_V2()
        {
            // Arrange
            string[] program = new[]
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1",
            };

            // Act
            long actual = Day14.RunProgram(program, version: 2);

            // Assert
            actual.ShouldBe(208);
        }

        [Fact]
        public async Task Y2020_Day14_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day14>();

            // Assert
            puzzle.SumOfRemainingValuesV1.ShouldBe(9967721333886L);
            puzzle.SumOfRemainingValuesV2.ShouldBeGreaterThan(696373757011L);
            puzzle.SumOfRemainingValuesV2.ShouldNotBe(702519779853L);
        }
    }
}
