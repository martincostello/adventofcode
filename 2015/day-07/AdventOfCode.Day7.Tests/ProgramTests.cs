// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgramTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   ProgramTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class that contains tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Fact]
        public static void InstructionsAreInterpretedCorrectly()
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
            IDictionary<string, ushort> actual = Program.GetWireValues(instructions);

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
