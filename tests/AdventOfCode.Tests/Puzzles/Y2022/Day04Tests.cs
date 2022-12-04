// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day04Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day04Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day04Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(false, 2)]
    [InlineData(true, 4)]
    public void Y2022_Day04_GetOverlappingAssignments_Returns_Correct_Values(bool partial, int expected)
    {
        // Arrange
        string[] assignments = new[]
        {
            "2-4,6-8",
            "2-3,4-5",
            "5-7,7-9",
            "2-8,3-7",
            "6-6,4-6",
            "2-6,4-8",
        };

        // Act
        int actual = Day04.GetOverlappingAssignments(assignments, partial);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.FullyOverlappingAssignments.ShouldBe(526);
        puzzle.PartiallyOverlappingAssignments.ShouldBe(886);
    }
}
