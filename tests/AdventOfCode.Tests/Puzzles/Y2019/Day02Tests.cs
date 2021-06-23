// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day02Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day02Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day02Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("1,9,10,3,2,3,11,0,99,30,40,50", new[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50L })]
        [InlineData("1,0,0,0,99", new[] { 2, 0, 0, 0, 99L })]
        [InlineData("2,3,0,3,99", new[] { 2, 3, 0, 6, 99L })]
        [InlineData("2,4,4,5,99,0", new[] { 2, 4, 4, 5, 99, 9801L })]
        [InlineData("1,1,1,4,99,5,6,0,99", new[] { 30, 1, 1, 4, 2, 5, 6, 0, 99L })]
        public async Task Y2019_Day02_RunProgram_Returns_Correct_State(string program, long[] expected)
        {
            // Act
            IReadOnlyList<long> actual = await Day02.RunProgramAsync(program, cancellationToken: CancellationToken.None);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2019_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day02>();

            // Assert
            puzzle.Output.ShouldNotBeNull();
            puzzle.Output.ShouldNotBeEmpty();
            puzzle.Output[0].ShouldBe(9581917);
        }
    }
}
