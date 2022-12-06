// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day06Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day06Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day06Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4, 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 4, 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 4, 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4, 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4, 11)]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14, 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 14, 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 14, 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14, 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14, 26)]
    public void Y2022_Day06_FindFirstPacket_Returns_Correct_Value(string datastream, int distinctCharacters, int expected)
    {
        // Act
        int actual = Day06.FindFirstPacket(datastream, distinctCharacters);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.IndexOfFirstStartOfPacketMarker.ShouldBe(1850);
        puzzle.IndexOfFirstStartOfMessageMarker.ShouldBe(2823);
    }
}
