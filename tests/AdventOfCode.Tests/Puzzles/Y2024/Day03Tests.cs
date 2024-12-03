// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", false, 161)]
    [InlineData("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", true, 48)]
    public void Y2024_Day03_Scan_Returns_Correct_Value(
        string memory,
        bool enhancedAccuracy,
        int expected)
    {
        // Act
        int actual = Day03.Scan(memory, enhancedAccuracy);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Sum.ShouldBe(165225049);
        puzzle.AccurateSum.ShouldBe(108830766);
    }
}
