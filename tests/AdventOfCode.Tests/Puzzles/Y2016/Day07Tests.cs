// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day07Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
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
