// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day11Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day11Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day11Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day11_GetMonkeyBusiness_Returns_Correct_Value()
    {
        // Arrange
        string[] observations = new[]
        {
            "Monkey 0:",
            "  Starting items: 79, 98",
            "  Operation: new = old * 19",
            "  Test: divisible by 23",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 3",
            string.Empty,
            "Monkey 1:",
            "  Starting items: 54, 65, 75, 74",
            "  Operation: new = old + 6",
            "  Test: divisible by 19",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 0",
            string.Empty,
            "Monkey 2:",
            "  Starting items: 79, 60, 97",
            "  Operation: new = old * old",
            "  Test: divisible by 13",
            "    If true: throw to monkey 1",
            "    If false: throw to monkey 3",
            string.Empty,
            "Monkey 3:",
            "  Starting items: 74",
            "  Operation: new = old + 3",
            "  Test: divisible by 17",
            "    If true: throw to monkey 0",
            "    If false: throw to monkey 1",
        };

        // Act
        long actual = Day11.GetMonkeyBusiness(observations, rounds: 20);

        // Assert
        actual.ShouldBe(10605);
    }

    [Fact]
    public async Task Y2022_Day11_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day11>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.MonkeyBusiness.ShouldBe(56120);
    }
}
