// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   InstructionTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Instruction"/> class. This class cannot be inherited.
    /// </summary>
    public static class InstructionTests
    {
        [Theory]
        [InlineData("turn on 0,0 through 999,999", "ON", 0, 0, 1000, 1000)]
        [InlineData("toggle 0,0 through 999,0", "TOGGLE", 0, 0, 1000, 1)]
        [InlineData("turn off 499,499 through 500,500", "OFF", 499, 499, 2, 2)]
        public static void InstructionsCanBeParsed(string value, string action, int x, int y, int width, int height)
        {
            Instruction actual = Instruction.Parse(value);

            Assert.Equal(action, actual.Action);
            Assert.Equal(x, actual.Bounds.X);
            Assert.Equal(y, actual.Bounds.Y);
            Assert.Equal(height, actual.Bounds.Height);
            Assert.Equal(width, actual.Bounds.Width);
        }
    }
}
