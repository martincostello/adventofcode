// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day10Tests : PuzzleTest
{
    public Day10Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day10_Compile_Returns_Correct_Value()
    {
        // Arrange
        string[] lines =
        {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]",
        };

        // Act
        (int syntaxErrorScore, long middleAutoCompleteScore) = Day10.Compile(lines);

        // Assert
        syntaxErrorScore.ShouldBe(26397);
        middleAutoCompleteScore.ShouldBe(288957);
    }

    [Fact]
    public async Task Y2021_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.SyntaxErrorScore.ShouldBe(323613);
        puzzle.MiddleAutoCompleteScore.ShouldBe(3103006161);
    }
}
