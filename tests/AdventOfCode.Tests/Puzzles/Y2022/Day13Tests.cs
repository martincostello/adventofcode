// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

public sealed class Day13Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2022_Day13_DecodePackets_Returns_Correct_Value()
    {
        // Arrange
        string[] packets =
        [
            "[1,1,3,1,1]",
            "[1,1,5,1,1]",
            string.Empty,
            "[[1],[2,3,4]]",
            "[[1],4]",
            string.Empty,
            "[9]",
            "[[8,7,6]]",
            string.Empty,
            "[[4,4],4,4]",
            "[[4,4],4,4,4]",
            string.Empty,
            "[7,7,7,7]",
            "[7,7,7]",
            string.Empty,
            "[]",
            "[3]",
            string.Empty,
            "[[[]]]",
            "[[]]",
            string.Empty,
            "[1,[2,[3,[4,[5,6,7]]]],8,9]",
            "[1,[2,[3,[4,[5,6,0]]]],8,9]",
        ];

        // Act
        (int actualSum, int actualDecoderKey) = Day13.DecodePackets(packets);

        // Assert
        actualSum.ShouldBe(13);
        actualDecoderKey.ShouldBe(140);
    }

    [Fact]
    public async Task Y2022_Day13_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day13>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfPresortedIndicies.ShouldBe(5252);
        puzzle.DecoderKey.ShouldBe(20592);
    }
}
