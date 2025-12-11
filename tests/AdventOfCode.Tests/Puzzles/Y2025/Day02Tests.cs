// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("11-22", false, 11 + 22)]
    [InlineData("11-22", true, 11 + 22)]
    [InlineData("95-115", false, 99)]
    [InlineData("95-115", true, 99 + 111)]
    [InlineData("998-1012", false, 1010)]
    [InlineData("998-1012", true, 999 + 1010)]
    [InlineData("1188511880-1188511890", false, 1188511885)]
    [InlineData("1188511880-1188511890", true, 1188511885)]
    [InlineData("222220-222224", false, 222222)]
    [InlineData("222220-222224", true, 222222)]
    [InlineData("1698522-1698528", false, 0)]
    [InlineData("1698522-1698528", true, 0)]
    [InlineData("446443-446449", false, 446446)]
    [InlineData("446443-446449", true, 446446)]
    [InlineData("38593856-38593862", false, 38593859)]
    [InlineData("38593856-38593862", true, 38593859)]
    [InlineData("565653-565659", false, 0)]
    [InlineData("565653-565659", true, 565656)]
    [InlineData("824824821-824824827", false, 0)]
    [InlineData("824824821-824824827", true, 824824824)]
    [InlineData("2121212118-2121212124", false, 0)]
    [InlineData("2121212118-2121212124", true, 2121212121)]
    [InlineData("11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124", false, 1227775554)]
    [InlineData("11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124", true, 4174379265)]
    public void Y2025_Day02_Validate_Returns_Correct_Value(string value, bool anyRepeatingSequence, long expected)
    {
        // Act
        long actual = Day02.Validate(value, anyRepeatingSequence);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2025_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.Solution1.ShouldBe(21898734247);
        puzzle.Solution2.ShouldBe(28915664389);
    }
}
