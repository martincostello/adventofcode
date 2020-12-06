// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day01Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day01Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day01Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

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
        public static void Y2015_Day01_GetFinalFloorAndFirstInstructionBasementReached(string value, int expectedFloor, int expectedInstruction)
        {
            // Act
            (int actualFloor, int actualInstruction) = Day01.GetFinalFloorAndFirstInstructionBasementReached(value);

            // Assert
            actualFloor.ShouldBe(expectedFloor);
            actualInstruction.ShouldBe(expectedInstruction);
        }

        [Fact]
        public async Task Y2015_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day01>();

            // Assert
            puzzle.FinalFloor.ShouldBe(232);
            puzzle.FirstBasementInstruction.ShouldBe(1783);
        }
    }
}
