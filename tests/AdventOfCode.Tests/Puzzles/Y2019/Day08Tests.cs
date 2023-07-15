// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("123456789012", 2, 3, 1)]
    [InlineData("0222112222120000", 2, 2, 4)]
    public void Y2019_Day08_GetImageChecksum_Returns_Correct_Output(string program, int height, int width, int expected)
    {
        // Act
        (int actual, _, _) = Day08.GetImageChecksum(program, height, width);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2019_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.Checksum.ShouldBe(2080);
        puzzle.Message.ShouldBe("AURCY");
    }
}
