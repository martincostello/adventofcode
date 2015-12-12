// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Impl;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day05Tests
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
        public static void Day05_IsStringNiceV1(string value, bool expected)
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
        public static void Day05_IsStringNiceV2(string value, bool expected)
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
        public static void Day05_HasPairOfLettersWithMoreThanOneOccurence(string value, bool expected)
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
        public static void Day05_HasLetterThatIsTheBreadOfALetterSandwich(string value, bool expected)
        {
            // Act
            bool actual = NicenessRuleV2.HasLetterThatIsTheBreadOfALetterSandwich(value);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
