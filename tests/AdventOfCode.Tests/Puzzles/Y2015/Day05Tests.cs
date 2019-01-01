// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day05Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day05Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day05Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("ugknbfddgicrmopn", true)]
        [InlineData("aaa", true)]
        [InlineData("jchzalrnumimnmhp", false)]
        [InlineData("haegwjzuvuyypxyu", false)]
        [InlineData("dvszwmarrgswjxmb", false)]
        [InlineData("haegwjzuvuyypabu", false)]
        [InlineData("haegwjzuvuyypcdu", false)]
        [InlineData("haegwjzuvuyyppqu", false)]
        public static void Y2015_Day05_IsNiceV1(string value, bool expected)
        {
            // Act
            bool actual = Day05.IsNiceV1(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("qjhvhtzxzqqjkmpb", true)]
        [InlineData("xxyxx", true)]
        [InlineData("uurcxstgmygtbstg", false)]
        [InlineData("ieodomkazucvgmuy", false)]
        public static void Y2015_Day05_IsNiceV2(string value, bool expected)
        {
            // Act
            bool actual = Day05.IsNiceV2(value);

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
        public static void Y2015_Day05_HasPairOfLettersWithMoreThanOneOccurence(string value, bool expected)
        {
            // Act
            bool actual = Day05.HasPairOfLettersWithMoreThanOneOccurence(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("xyx", true)]
        [InlineData("abcdefeghi", true)]
        [InlineData("aaa", true)]
        [InlineData("a", false)]
        [InlineData("aa", false)]
        public static void Y2015_Day05_HasLetterThatIsTheBreadOfALetterSandwich(string value, bool expected)
        {
            // Act
            bool actual = Day05.HasLetterThatIsTheBreadOfALetterSandwich(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2015_Day05_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "1" };

            // Act
            var puzzle = SolvePuzzle<Day05>(args);

            // Assert
            Assert.Equal(236, puzzle.NiceStringCount);

            // Arrange
            args = new[] { "2" };

            // Act
            puzzle = SolvePuzzle<Day05>(args);

            // Assert
            Assert.Equal(51, puzzle.NiceStringCount);
        }
    }
}
