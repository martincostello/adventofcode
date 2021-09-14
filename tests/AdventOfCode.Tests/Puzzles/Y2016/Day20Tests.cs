// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class containing tests for the <see cref="Day20"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day20Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day20Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day20Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(9, new[] { "5-8", "0-2", "4-7" }, 3, 2)]
    public static void Y2016_Day20_GetLowestNonblockedIP_Returns_Correct_Solution(uint maxValue, string[] denyList, uint expectedIP, uint expectedCount)
    {
        // Act
        uint address = Day20.GetLowestNonblockedIP(maxValue, denyList, out uint count);

        // Assert
        address.ShouldBe(expectedIP);
        count.ShouldBe(expectedCount);
    }

    [Fact]
    public async Task Y2016_Day20_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day20>();

        // Assert
        puzzle.LowestNonblockedIP.ShouldBe(22887907u);
        puzzle.AllowedIPCount.ShouldBe(109u);
    }
}
