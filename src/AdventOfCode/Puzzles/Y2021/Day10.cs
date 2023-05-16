// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 10, "Syntax Scoring", RequiresData = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the syntax error score for the navigation subsystem.
    /// </summary>
    public int SyntaxErrorScore { get; private set; }

    /// <summary>
    /// Gets the middle auto-complete score for the navigation subsystem.
    /// </summary>
    public long MiddleAutoCompleteScore { get; private set; }

    /// <summary>
    /// Compiles the specified lines of the navigation subsystem.
    /// </summary>
    /// <param name="lines">The lines of chunks to compile.</param>
    /// <returns>
    /// The syntax error score and the middle auto-complete score for the navigation subsystem.
    /// </returns>
    public static (int SyntaxErrorScore, long MiddleAutoCompleteScore) Compile(IList<string> lines)
    {
        var pairs = new Dictionary<char, char>(8)
        {
            ['('] = ')',
            ['['] = ']',
            ['{'] = '}',
            ['<'] = '>',
            [')'] = '(',
            [']'] = '[',
            ['}'] = '{',
            ['>'] = '<',
        };

        var completeScoreMap = new Dictionary<char, int>(4)
        {
            [')'] = 1,
            [']'] = 2,
            ['}'] = 3,
            ['>'] = 4,
        };

        var errorScoreMap = new Dictionary<char, int>(4)
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };

        var autoCompleteScores = new List<long>();
        var errorScores = new List<int>();

        foreach (string line in lines)
        {
            bool isCorrupted = false;
            char illegal = default;
            var chunks = new Stack<char>();

            foreach (char value in line)
            {
                char expected = default;

                switch (value)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        chunks.Push(value);
                        continue;

                    case ')':
                    case ']':
                    case '}':
                    case '>':
                    default:
                        expected = pairs[value];
                        break;
                }

                if (chunks.Peek() != expected)
                {
                    illegal = value;
                    isCorrupted = true;
                    break;
                }

                chunks.Pop();
            }

            if (isCorrupted)
            {
                errorScores.Add(errorScoreMap[illegal]);
            }
            else
            {
                long score = 0;

                foreach (char value in chunks)
                {
                    score *= 5;
                    score += completeScoreMap[pairs[value]];
                }

                autoCompleteScores.Add(score);
            }
        }

        int syntaxErrorScore = errorScores.Sum();

        autoCompleteScores.Sort();

        long middleAutoCompleteScore = autoCompleteScores[autoCompleteScores.Count / 2];

        return (syntaxErrorScore, middleAutoCompleteScore);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var lines = await ReadResourceAsLinesAsync(cancellationToken);

        (SyntaxErrorScore, MiddleAutoCompleteScore) = Compile(lines);

        if (Verbose)
        {
            Logger.WriteLine("The total syntax error score is {0:N0}.", SyntaxErrorScore);
            Logger.WriteLine("The middle auto-complete score is {0:N0}.", MiddleAutoCompleteScore);
        }

        return PuzzleResult.Create(SyntaxErrorScore, MiddleAutoCompleteScore);
    }
}
