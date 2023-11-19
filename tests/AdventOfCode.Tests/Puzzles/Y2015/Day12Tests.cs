// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.Json;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("[1,2,3]", null, 6)]
    [InlineData(@"{""a"":2,""b"":4}", null, 6)]
    [InlineData("[[[3]]]", null, 3)]
    [InlineData(@"{""a"":{""b"":4},""c"":-1}", null, 3)]
    [InlineData(@"{""a"":[-1,1]}", null, 0)]
    [InlineData(@"[-1,{""a"":1}]", null, 0)]
    [InlineData("[]", null, 0)]
    [InlineData("{}", null, 0)]
    [InlineData("[1,2,3]", "red", 6)]
    [InlineData(@"[1,{""c"":""red"",""b"":2},3]", "red", 4)]
    [InlineData(@"{""d"":""red"",""e"":[1,2,3,4],""f"":5}", "red", 0)]
    [InlineData(@"[1,""red"",5]", "red", 6)]
    public static void Y2015_Day12_SumIntegerValues(string json, string? keyToIgnore, long expected)
    {
        // Arrange
        using var document = JsonDocument.Parse(json);

        // Act
        long actual = Day12.SumIntegerValues(document.RootElement, keyToIgnore);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.Sum.ShouldBe(191164);
        puzzle.SumIgnoringRed.ShouldBe(87842);
    }
}
