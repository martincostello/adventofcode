// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day04Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day04Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day04Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("aa bb cc dd ee", 1, true)]
    [InlineData("aa bb cc dd aa", 1, false)]
    [InlineData("aa bb cc dd aaa", 1, true)]
    [InlineData("abcde fghij", 2, true)]
    [InlineData("abcde xyz ecdab", 2, false)]
    [InlineData("a ab abc abd abf abj", 2, true)]
    [InlineData("iiii oiii ooii oooi oooo", 2, true)]
    [InlineData("oiii ioii iioi iiio", 2, false)]
    public static void Y2017_Day04_IsPassphraseValid_Returns_Correct_Value(string passphrase, int version, bool expected)
    {
        // Act
        bool actual = Day04.IsPassphraseValid(passphrase, version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.ValidPassphraseCountV1.ShouldBe(383);
        puzzle.ValidPassphraseCountV2.ShouldBe(265);
    }
}
