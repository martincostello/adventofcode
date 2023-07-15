// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("{}", 1)]
    [InlineData("{{{}}}", 6)]
    [InlineData("{{},{}}", 5)]
    [InlineData("{{{},{},{{}}}}", 16)]
    [InlineData("{<{},{},{{}}>}", 1)]
    [InlineData("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9)]
    [InlineData("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9)]
    [InlineData("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3)]
    public static void Y2017_Day09_ParseStream_Returns_Correct_Value_For_Score(string stream, int expected)
    {
        // Act
        (int actual, int _) = Day09.ParseStream(stream);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("<>", 0)]
    [InlineData("<random characters>", 17)]
    [InlineData("<<<<>", 3)]
    [InlineData("<{!>}>", 2)]
    [InlineData("<!!>", 0)]
    [InlineData("<!!!>>", 0)]
    [InlineData("<{o\"i!a,<{i<a>", 10)]
    public static void Y2017_Day09_ParseStream_Returns_Correct_Value_For_Garbage(string stream, int expected)
    {
        // Act
        (int _, int actual) = Day09.ParseStream(stream);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.TotalScore.ShouldBe(11898);
        puzzle.GarbageCount.ShouldBe(5601);
    }
}
