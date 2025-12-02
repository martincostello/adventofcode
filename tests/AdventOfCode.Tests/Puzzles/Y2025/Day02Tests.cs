// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("11-22", 33)]
    [InlineData("95-115", 99)]
    [InlineData("998-1012", 1010)]
    [InlineData("1188511880-1188511890", 1188511885)]
    [InlineData("222220-222224", 222222)]
    [InlineData("1698522-1698528", 0)]
    [InlineData("446443-446449", 446446)]
    [InlineData("38593856-38593862", 38593859)]
    [InlineData("11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124", 1227775554)]
    public void Y2025_Day02_Validate_Returns_Correct_Value(string value, long expected)
    {
        // Act
        long actual = Day02.Validate(value);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2025_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.InvalidIdSum.ShouldBe(21898734247);
    }
}
