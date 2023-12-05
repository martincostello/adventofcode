// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 35)]
    [InlineData(true, 46)]
    public void Y2023_Day05_Parse_Returns_Correct_Value(bool useRanges, int expected)
    {
        // Arrange
        string[] almanac =
        [
            "seeds: 79 14 55 13",
            string.Empty,
            "seed-to-soil map:",
            "50 98 2",
            "52 50 48",
            string.Empty,
            "soil-to-fertilizer map:",
            "0 15 37",
            "37 52 2",
            "39 0 15",
            string.Empty,
            "fertilizer-to-water map:",
            "49 53 8",
            "0 11 42",
            "42 0 7",
            "57 7 4",
            string.Empty,
            "water-to-light map:",
            "88 18 7",
            "18 25 70",
            string.Empty,
            "light-to-temperature map:",
            "45 77 23",
            "81 45 19",
            "68 64 13",
            string.Empty,
            "temperature-to-humidity map:",
            "0 69 1",
            "1 0 69",
            string.Empty,
            "humidity-to-location map:",
            "60 56 37",
            "56 93 4",
        ];

        // Act
        long actual = Day05.Parse(almanac, useRanges);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.LocationMinimum.ShouldBe(535088217);
        puzzle.LocationMinimumWithRanges.ShouldBe(51399228);
    }
}
