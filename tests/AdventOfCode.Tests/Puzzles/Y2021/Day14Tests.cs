// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(10, 1588)]
    [InlineData(40, 2188189693529)]
    public void Y2021_Day14_Expand_Returns_Correct_Value(int steps, long expected)
    {
        // Arrange
        string[] instructions =
        {
            "NNCB",
            string.Empty,
            "CH -> B",
            "HH -> N",
            "CB -> H",
            "NH -> C",
            "HB -> C",
            "HC -> B",
            "HN -> C",
            "NN -> C",
            "BH -> H",
            "NC -> B",
            "NB -> B",
            "BN -> B",
            "BB -> N",
            "BC -> B",
            "CC -> N",
            "CN -> C",
        };

        // Act
        long actual = Day14.Expand(instructions, steps);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.Score10.ShouldBe(3587);
        puzzle.Score40.ShouldBe(3906445077999);
    }
}
