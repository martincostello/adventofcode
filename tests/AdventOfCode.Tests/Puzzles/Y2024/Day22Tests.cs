// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day22Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("123", 15887950)]
    [InlineData("15887950", 16495136)]
    [InlineData("16495136", 527345)]
    [InlineData("527345", 704524)]
    [InlineData("704524", 1553684)]
    [InlineData("1553684", 12683156)]
    [InlineData("12683156", 11100544)]
    [InlineData("11100544", 12249484)]
    [InlineData("12249484", 7753432)]
    [InlineData("7753432", 5908254)]
    public void Y2024_Day22_Simulate_One_Returns_Correct_Value(string value, long expected)
    {
        // Arrange
        string[] values = [value];

        // Act
        long actual = Day22.Simulate(values, rounds: 1);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Y2024_Day22_Simulate_Many_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "1",
            "10",
            "100",
            "2024",
        ];

        // Act
        long actual = Day22.Simulate(values, rounds: 2000);

        // Assert
        actual.ShouldBe(37327623);
    }

    [Fact]
    public async Task Y2024_Day22_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day22>();

        // Assert
        puzzle.Solution.ShouldBe(14623556510);
    }
}
