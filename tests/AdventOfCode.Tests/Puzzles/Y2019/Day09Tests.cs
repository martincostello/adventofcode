// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", new[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99L })]
    [InlineData("1102,34915192,34915192,7,4,7,99,0", new[] { 1219070632396864 })]
    [InlineData("104,1125899906842624,99", new[] { 1125899906842624 })]
    public async Task Y2019_Day09_RunProgram_Returns_Correct_Output(
        string program,
        long[] expected)
    {
        // Act
        IReadOnlyList<long> actual = await Day09.RunProgramAsync(program, cancellationToken: CancellationToken.None);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2019_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.Keycode1.ShouldBe(2494485073);
        puzzle.Keycode2.ShouldBe(44997);
    }
}
