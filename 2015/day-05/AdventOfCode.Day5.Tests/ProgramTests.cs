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
        public static void IsStringNiceV1(string value, bool expected)
        {
            // Arrange
            var target = new NicenessRuleV1();

            // Act
            bool actual = target.IsNice(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("qjhvhtzxzqqjkmpb", true)]
        [InlineData("xxyxx", true)]
        [InlineData("uurcxstgmygtbstg", false)]
        [InlineData("ieodomkazucvgmuy", false)]
        public static void IsStringNiceV2(string value, bool expected)
        {
            // Arrange
            var target = new NicenessRuleV2();

            // Act
            bool actual = target.IsNice(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("xyxy", true)]
        [InlineData("aabcdefgaa", true)]
        [InlineData("abaaab", true)]
        [InlineData("a", false)]
        [InlineData("aa", false)]
        [InlineData("aaa", false)]
        [InlineData("aaaa", true)]
        public static void HasPairOfLettersWithMoreThanOneOccurence(string value, bool expected)
        {
            // Act
            bool actual = NicenessRuleV2.HasPairOfLettersWithMoreThanOneOccurence(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("xyx", true)]
        [InlineData("abcdefeghi", true)]
        [InlineData("aaa", true)]
        [InlineData("a", false)]
        [InlineData("aa", false)]
        public static void HasLetterThatIsTheBreadOfALetterSandwich(string value, bool expected)
        {
            // Act
            bool actual = NicenessRuleV2.HasLetterThatIsTheBreadOfALetterSandwich(value);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
