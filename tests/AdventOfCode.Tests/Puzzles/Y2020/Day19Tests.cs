// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day19Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day19Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day19Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Y2020_Day19_GetMatchCount_Returns_Correct_Value()
        {
            // Arrange
            string[] input = new[]
            {
                "0: 4 1 5",
                "1: 2 3 | 3 2",
                "2: 4 4 | 5 5",
                "3: 4 5 | 5 4",
                "4: \"a\"",
                "5: \"b\"",
                string.Empty,
                "ababbb",
                "bababa",
                "abbbab",
                "aaabbb",
                "aaaabbb",
            };

            // Act
            int actual = Day19.GetMatchCount(input, ruleIndex: 0);

            // Assert
            actual.ShouldBe(2);
        }

        [Fact]
        public async Task Y2020_Day19_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day19>();

            // Assert
            puzzle.MatchesRule0.ShouldBe(195);
        }
    }
}
