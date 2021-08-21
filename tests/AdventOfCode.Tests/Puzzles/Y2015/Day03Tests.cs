// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day03Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day03Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day03Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(">", 1)]
    [InlineData("^>v<", 4)]
    [InlineData("^v^v^v^v^v", 2)]
    public void Y2015_Day03_GetUniqueHousesVisitedBySanta(string instructions, int expected)
    {
        // Act
        int actual = Day03.GetUniqueHousesVisitedBySanta(instructions, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("^v", 3)]
    [InlineData("^>v<", 3)]
    [InlineData("^v^v^v^v^v", 11)]
    public void Y2015_Day03_GetUniqueHousesVisitedBySantaAndRoboSanta(string instructions, int expected)
    {
        // Act
        int actual = Day03.GetUniqueHousesVisitedBySantaAndRoboSanta(instructions, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.HousesWithPresentsFromSanta.ShouldBe(2565);
        puzzle.HousesWithPresentsFromSantaAndRoboSanta.ShouldBe(2639);
    }
}
