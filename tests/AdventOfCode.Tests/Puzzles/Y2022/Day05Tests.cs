// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, "CMZ")]
    [InlineData(true, "MCD")]
    public void Y2022_Day05_RearrangeCrates_Returns_Correct_Value(bool canMoveMultipleCrates, string expected)
    {
        // Arrange
        string[] instructions = new[]
        {
            "    [D]     ",
            "[N] [C]     ",
            "[Z] [M] [P] ",
            " 1   2   3  ",
            string.Empty,
            "move 1 from 2 to 1",
            "move 3 from 1 to 3",
            "move 2 from 2 to 1",
            "move 1 from 1 to 2",
        };

        // Act
        string actual = Day05.RearrangeCrates(instructions, canMoveMultipleCrates);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TopCratesOfStacks9000.ShouldBe("TGWSMRBPN");
        puzzle.TopCratesOfStacks9001.ShouldBe("TZLTLWRNF");
    }
}
