﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day07Tests
    {
        [Theory]
        [InlineData("aba[bab]xyz", true)]
        [InlineData("xyx[xyx]xyx", false)]
        [InlineData("aaa[kek]eke", true)]
        [InlineData("zazbz[bzb]cdb", true)]
        public static void Y2016_Day07_DoesIPAddressSupportSsl_Returns_Correct_Solution(string address, bool expected)
        {
            // Act
            bool actual = Day07.DoesIPAddressSupportSsl(address);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("abba[mnop]qrst", true)]
        [InlineData("abcd[bddb]xyyx", false)]
        [InlineData("aaaa[qwer]tyui", false)]
        [InlineData("ioxxoj[asdfgh]zxcvbn", true)]
        public static void Y2016_Day07_DoesIPAddressSupportTls_Returns_Correct_Solution(string address, bool expected)
        {
            // Act
            bool actual = Day07.DoesIPAddressSupportTls(address);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day07>();

            // Assert
            Assert.Equal(118, puzzle.IPAddressesSupportingTls);
            Assert.Equal(260, puzzle.IPAddressesSupportingSsl);
        }
    }
}
