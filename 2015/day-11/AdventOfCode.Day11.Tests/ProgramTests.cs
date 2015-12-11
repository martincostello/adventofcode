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

namespace MartinCostello.AdventOfCode.Day11
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Theory]
        [InlineData("abcdefgh", "abcdffaa")]
        [InlineData("ghijklmn", "ghjaabcc")]
        public static void GenerateNextPassword(string current, string expected)
        {
            // Act
            string actual = Program.GenerateNextPassword(current);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
