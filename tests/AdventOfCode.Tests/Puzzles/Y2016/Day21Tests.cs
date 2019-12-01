// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day21"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day21Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day21Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day21Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(new[] { 'a', 'b', 'c', 'd', 'f', 'e', 'g', 'h' }, 2, 5, new[] { 'a', 'b', 'e', 'f', 'd', 'c', 'g', 'h' })]
        public static void Y2016_Day21_Reverse_Returns_Correct_Solution(char[] values, int start, int end, IEnumerable<char> expected)
        {
            // Act
            Day21.Reverse(values, start, end);

            // Assert
            values.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { 'a', 'b', 'c' }, false, 1, new[] { 'b', 'c', 'a' })]
        [InlineData(new[] { 'a', 'b', 'c' }, true, 1, new[] { 'c', 'a', 'b' })]
        [InlineData(new[] { 'a', 'b', 'c', 'd' }, false, 1, new[] { 'b', 'c', 'd', 'a' })]
        [InlineData(new[] { 'a', 'b', 'c', 'd' }, true, 1, new[] { 'd', 'a', 'b', 'c' })]
        [InlineData(new[] { 'a', 'b', 'c' }, false, 2, new[] { 'c', 'a', 'b' })]
        [InlineData(new[] { 'a', 'b', 'c' }, true, 2, new[] { 'b', 'c', 'a' })]
        [InlineData(new[] { 'a', 'b', 'c' }, false, 3, new[] { 'a', 'b', 'c' })]
        [InlineData(new[] { 'a', 'b', 'c' }, true, 3, new[] { 'a', 'b', 'c' })]
        public static void Y2016_Day21_RotateDirection_Returns_Correct_Solution(char[] values, bool right, int steps, IEnumerable<char> expected)
        {
            // Act
            Day21.RotateDirection(values, right, steps, reverse: false);

            // Assert
            values.ShouldBe(expected);
        }

        [Theory]
        [InlineData("abcde", false, "decab")]
        [InlineData("decab", true, "abcde")]
        public static void Y2016_Day21_Scramble_Returns_Correct_Solution(string text, bool reverse, string expected)
        {
            // Arrange
            string[] instructions = new[]
            {
                "swap position 4 with position 0",
                "swap letter d with letter b",
                "reverse positions 0 through 4",
                "rotate left 1 step",
                "move position 1 to position 4",
                "move position 3 to position 0",
                "rotate based on position of letter b",
                "rotate based on position of letter d",
            };

            // Act
            string actual = Day21.Scramble(text, instructions, reverse);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { "abcdefgh" }, "gcedfahb")]
        [InlineData(new[] { "fbgdceah", "true" }, "hegbdcfa")]
        public void Y2016_Day21_Solve_Returns_Correct_Solution(string[] args, string expected)
        {
            // Act
            var puzzle = SolvePuzzle<Day21>(args);

            // Assert
            puzzle.ScrambledResult.ShouldBe(expected);
        }
    }
}
