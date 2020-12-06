// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day07Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day07Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day07Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

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
            actual.ShouldBe(expected);
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
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day07>();

            // Assert
            puzzle.IPAddressesSupportingTls.ShouldBe(118);
            puzzle.IPAddressesSupportingSsl.ShouldBe(260);
        }
    }
}
