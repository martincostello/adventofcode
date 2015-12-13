// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Newtonsoft.Json.Linq;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day12"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day12Tests
    {
        [Theory]
        [InlineData("[1,2,3]", 6)]
        [InlineData(@"{""a"":2,""b"":4}", 6)]
        [InlineData("[[[3]]]", 3)]
        [InlineData(@"{""a"":{""b"":4},""c"":-1}", 3)]
        [InlineData(@"{""a"":[-1,1]}", 0)]
        [InlineData(@"[-1,{""a"":1}]", 0)]
        [InlineData("[]", 0)]
        [InlineData("{}", 0)]
        public static void SumIntegerValues(string json, long expected)
        {
            // Arrange
            JToken token = JToken.Parse(json);

            // Act
            long actual = Day12.SumIntegerValues(token);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
