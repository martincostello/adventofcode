// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionV2Tests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   InstructionV2Tests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="InstructionV2"/> class. This class cannot be inherited.
    /// </summary>
    public static class InstructionV2Tests
    {
        [Theory]
        [InlineData("turn on 0,0 through 999,999", 1, 0, 0, 1000, 1000)]
        [InlineData("toggle 0,0 through 999,0", 2, 0, 0, 1000, 1)]
        [InlineData("turn off 499,499 through 500,500", -1, 499, 499, 2, 2)]
        public static void V2InstructionsCanBeParsed(string value, int delta, int x, int y, int width, int height)
        {
            InstructionV2 actual = InstructionV2.Parse(value);

            Assert.Equal(delta, actual.Delta);
            Assert.Equal(x, actual.Bounds.X);
            Assert.Equal(y, actual.Bounds.Y);
            Assert.Equal(height, actual.Bounds.Height);
            Assert.Equal(width, actual.Bounds.Width);
        }
    }
}
