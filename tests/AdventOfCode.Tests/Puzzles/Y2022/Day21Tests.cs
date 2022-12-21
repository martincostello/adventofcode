// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day21"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day21Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day21Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day21Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(false, 152)]
    [InlineData(true, 301)]
    public void Y2022_Day21_GetRootNumber_Returns_Correct_Value(bool withEquality, long expected)
    {
        // Arrange
        string[] jobs = new[]
        {
            "root: pppw + sjmn",
            "dbpl: 5",
            "cczh: sllz + lgvd",
            "zczc: 2",
            "ptdq: humn - dvpt",
            "dvpt: 3",
            "lfqf: 4",
            "humn: 5",
            "ljgn: 2",
            "sjmn: drzm * dbpl",
            "sllz: 4",
            "pppw: cczh / lfqf",
            "lgvd: ljgn * ptdq",
            "drzm: hmdt - zczc",
            "hmdt: 32",
        };

        // Act
        long actual = Day21.GetRootNumber(jobs, withEquality);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day21_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day21>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.RootMonkeyNumber.ShouldBe(10037517593724);
        puzzle.HumanNumber.ShouldBe(3272260914328);
    }
}
