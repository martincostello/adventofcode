// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day16Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day16Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day16Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day16_GetMaximumPressure_Returns_Correct_Value()
    {
        // Arrange
        string[] report = new[]
        {
            "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
            "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
            "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
            "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
            "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
            "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
            "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
            "Valve HH has flow rate=22; tunnel leads to valve GG",
            "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
            "Valve JJ has flow rate=21; tunnel leads to valve II",
        };

        // Act
        int actual = Day16.GetMaximumPressure(report);

        // Assert
        actual.ShouldBe(1651);
    }

    [Fact]
    public async Task Y2022_Day16_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day16>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MaximumPressure.ShouldBe(-1);
    }
}
