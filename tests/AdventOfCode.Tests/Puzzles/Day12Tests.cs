// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using Newtonsoft.Json.Linq;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day12"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day12Tests
    {
        [Theory]
        [InlineData("[1,2,3]", null, 6)]
        [InlineData(@"{""a"":2,""b"":4}", null, 6)]
        [InlineData("[[[3]]]", null, 3)]
        [InlineData(@"{""a"":{""b"":4},""c"":-1}", null, 3)]
        [InlineData(@"{""a"":[-1,1]}", null, 0)]
        [InlineData(@"[-1,{""a"":1}]", null, 0)]
        [InlineData("[]", null, 0)]
        [InlineData("{}", null, 0)]
        [InlineData("[1,2,3]", "red", 6)]
        [InlineData(@"[1,{""c"":""red"",""b"":2},3]", "red", 4)]
        [InlineData(@"{""d"":""red"",""e"":[1,2,3,4],""f"":5}", "red", 0)]
        [InlineData(@"[1,""red"",5]", "red", 6)]
        public static void Day12_SumIntegerValues(string json, string keyToIgnore, long expected)
        {
            // Arrange
            JToken token = JToken.Parse(json);

            // Act
            long actual = Day12.SumIntegerValues(token, keyToIgnore);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day12_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day12>();

            // Assert
            Assert.Equal(191164, puzzle.Sum);

            // Arrange
            string[] args = new[] { "red" };

            // Act
            puzzle = PuzzleTestHelpers.SolvePuzzle<Day12>(args);

            // Assert
            Assert.Equal(87842, puzzle.Sum);
        }
    }
}
