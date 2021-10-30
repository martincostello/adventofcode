// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day07Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day07Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day07Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", false, 43210)]
    [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", false, 54321)]
    [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", false, 65210)]
    [InlineData("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", true, 139629729)]
    [InlineData("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", true, 18216)]
    public async Task Y2019_Day07_RunProgram_Returns_Correct_Output(string program, bool useFeedback, long expected)
    {
        // Act
        long actual = await Day07.RunProgramAsync(program, useFeedback, CancellationToken.None);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2019_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.HighestSignal.ShouldBe(77500);
        puzzle.HighestSignalUsingFeedback.ShouldBe(22476942);
    }
}
