// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

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
    [InlineData(false, 157)]
    [InlineData(true, 70)]
    public void Y2022_Day03_GetSumOfCommonItemTypes_Returns_Correct_Values(bool useGroups, int expected)
    {
        // Arrange
        string[] inventories = new[]
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw",
        };

        // Act
        int actual = Day03.GetSumOfCommonItemTypes(inventories, useGroups);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfPriorities.ShouldBe(7568);
        puzzle.SumOfPrioritiesOfGroups.ShouldBe(2780);
    }
}
