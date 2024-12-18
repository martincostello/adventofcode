// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static TheoryData<string, bool, long> Examples()
    {
        return new()
        {
            {
                "12345",
                false,
                60
            },
            {
                "2333133121414131402",
                false,
                1928
            },
            {
                "2333133121414131402",
                true,
                2858
            },
        };
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void Y2024_Day09_Defragment_Returns_Correct_Value(string map, bool contiguousFiles, long expected)
    {
        // Act
        long actual = Day09.Defragment(map, contiguousFiles);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.ChecksumV1.ShouldBe(6395800119709);
        puzzle.ChecksumV2.ShouldBe(-1);
    }
}
