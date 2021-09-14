// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day05Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day05Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day05Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("3,0,4,0,99", 0, 0)]
    [InlineData("3,0,4,0,99", 1, 1)]
    [InlineData("3,0,4,0,99", 2, 2)]
    [InlineData("1002,4,3,4,33", 0, 0)]
    [InlineData("1101,100,-1,4,0", 0, 0)]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 0, 0)]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 7, 0)]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 8, 1)]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 9, 0)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 0, 1)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 7, 1)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 8, 0)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 9, 0)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 0, 0)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 7, 0)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 8, 1)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 9, 0)]
    [InlineData("3,3,1107,-1,8,3,4,3,99", 0, 1)]
    [InlineData("3,3,1107,-1,8,3,4,3,99", 7, 1)]
    [InlineData("3,3,1107,-1,8,3,4,3,99", 8, 0)]
    [InlineData("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 0)]
    [InlineData("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 1, 1)]
    [InlineData("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 0, 999)]
    [InlineData("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 7, 999)]
    [InlineData("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 8, 1000)]
    [InlineData("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 9, 1001)]
    public async Task Y2019_Day05_RunProgram_Returns_Correct_Output(string program, int input, long expected)
    {
        // Act
        long actual = await Day05.RunProgramAsync(program, input, CancellationToken.None);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("1", 6745903)]
    [InlineData("5", 9168267)]
    public async Task Y2019_Day05_Solve_Returns_Correct_Solution(string input, int expected)
    {
        // Arrange
        string[] args = { input };

        // Act
        var puzzle = await SolvePuzzleAsync<Day05>(args);

        // Assert
        puzzle.DiagnosticCode.ShouldBe(expected);
    }
}
