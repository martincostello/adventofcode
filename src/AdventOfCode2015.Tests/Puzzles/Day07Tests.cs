// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day07Tests
    {
        [Fact]
        public static void Day7_InstructionsAreInterpretedCorrectly()
        {
            // Arrange
            var instructions = new string[]
            {
                "j -> k",
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i",
                "i -> j",
            };

            // Act
            IDictionary<string, ushort> actual = Day07.GetWireValues(instructions);

            // Assert
            Assert.Equal(72, actual["d"]);
            Assert.Equal(507, actual["e"]);
            Assert.Equal(492, actual["f"]);
            Assert.Equal(114, actual["g"]);
            Assert.Equal(65412, actual["h"]);
            Assert.Equal(65079, actual["i"]);
            Assert.Equal(65079, actual["j"]);
            Assert.Equal(65079, actual["k"]);
            Assert.Equal(123, actual["x"]);
            Assert.Equal(456, actual["y"]);
        }
    }
}
