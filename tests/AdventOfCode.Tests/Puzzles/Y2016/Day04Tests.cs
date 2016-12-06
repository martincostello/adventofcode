// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day04Tests
    {
        [Theory]
        [InlineData("aaaaa-bbb-z-y-x-123[abxyz]", true)]
        [InlineData("a-b-c-d-e-f-g-h-987[abcde]", true)]
        [InlineData("not-a-real-room-404[oarel]", true)]
        [InlineData("totally-real-room-200[decoy]", false)]
        public static void Y2016_Day04_IsRoomReal_Returns_Correct_Solution(string name, bool expected)
        {
            // Act
            bool actual = Day04.IsRoomReal(name);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "aaaaa-bbb-z-y-x-123[abxyz]", "a-b-c-d-e-f-g-h-987[abcde]", "not-a-real-room-404[oarel]", "totally-real-room-200[decoy]" }, 1514)]
        public static void Y2016_Day04_SumOfRealRoomSectorIds_Returns_Correct_Solution(string[] names, int expected)
        {
            // Act
            int actual = Day04.SumOfRealRoomSectorIds(names);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day04_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day04>();

            // Assert
            Assert.Equal(137896, puzzle.SumOfSectorIdsOfRealRooms);
        }
    }
}
