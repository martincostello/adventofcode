// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircuitTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   CircuitTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class that contains tests for circuits. This class cannot be inherited.
    /// </summary>
    public static class CircuitTests
    {
        [Fact]
        public static void ExampleCircuitProducesExpectedOutput()
        {
            // Arrange and Act
            var x = new SimpleGate("x", 123);
            var y = new SimpleGate("y", 456);
            var d = new AndGate("d", x, y);
            var e = new OrGate("e", x, y);
            var f = new LeftShiftGate("f", x, 2);
            var g = new RightShiftGate("g", y, 2);
            var h = new NotGate("h", x);
            var i = new NotGate("i", y);

            // Assert
            Assert.Equal(72, d.Signal);
            Assert.Equal(507, e.Signal);
            Assert.Equal(492, f.Signal);
            Assert.Equal(114, g.Signal);
            Assert.Equal(65412, h.Signal);
            Assert.Equal(65079, i.Signal);
            Assert.Equal(123, x.Signal);
            Assert.Equal(456, y.Signal);
        }

        [Fact]
        public static void CircuitBuilderCreatesCorrectCircuit()
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
            IDictionary<string, ILogicGate> actual = CircuitBuilder.Build(instructions);

            // Assert
            Assert.Equal(72, actual["d"].Signal);
            Assert.Equal(507, actual["e"].Signal);
            Assert.Equal(492, actual["f"].Signal);
            Assert.Equal(114, actual["g"].Signal);
            Assert.Equal(65412, actual["h"].Signal);
            Assert.Equal(65079, actual["i"].Signal);
            Assert.Equal(65079, actual["j"].Signal);
            Assert.Equal(123, actual["x"].Signal);
            Assert.Equal(456, actual["y"].Signal);
        }
    }
}
