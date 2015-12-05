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

namespace MartinCostello.AdventOfCode.Day5
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Theory]
        [InlineData("ugknbfddgicrmopn", true)]
        [InlineData("aaa", true)]
        [InlineData("jchzalrnumimnmhp", false)]
        [InlineData("haegwjzuvuyypxyu", false)]
        [InlineData("dvszwmarrgswjxmb", false)]
        [InlineData("haegwjzuvuyypabu", false)]
        [InlineData("haegwjzuvuyypcdu", false)]
        [InlineData("haegwjzuvuyyppqu", false)]
        public static void Program_IsStringNice_Returns_Correct_Value(string value, bool expected)
        {
            // Act
            bool actual = Program.IsStringNice(value);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
