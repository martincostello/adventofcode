// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionV1Tests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   InstructionV1Tests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="InstructionV1"/> class. This class cannot be inherited.
    /// </summary>
    public static class InstructionV1Tests
    {
        [Theory]
        [InlineData("turn on 0,0 through 999,999", "ON", 0, 0, 1000, 1000)]
        [InlineData("toggle 0,0 through 999,0", "TOGGLE", 0, 0, 1000, 1)]
        [InlineData("turn off 499,499 through 500,500", "OFF", 499, 499, 2, 2)]
        public static void V1InstructionsCanBeParsed(string value, string action, int x, int y, int width, int height)
        {
            InstructionV1 actual = InstructionV1.Parse(value);

            Assert.Equal(action, actual.Action);
            Assert.Equal(x, actual.Bounds.X);
            Assert.Equal(y, actual.Bounds.Y);
            Assert.Equal(height, actual.Bounds.Height);
            Assert.Equal(width, actual.Bounds.Width);
        }
    }
}
