// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day08Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day08Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day08Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("123456789012", 2, 3, 1)]
    [InlineData("0222112222120000", 2, 2, 4)]
    public void Y2019_Day08_GetImageChecksum_Returns_Correct_Output(string program, int height, int width, int expected)
    {
        // Act
        (int actual, _) = Day08.GetImageChecksum(program, height, width);

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
    }
}
