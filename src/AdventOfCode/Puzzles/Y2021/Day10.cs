// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 10, RequiresData = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the syntax error score for the navigation subsystem.
    /// </summary>
    public int SyntaxErrorScore { get; private set; }

    /// <summary>
    /// Compiles the specified lines of the navigation subsystem.
    /// </summary>
    /// <param name="lines">The lines of chunks to compile.</param>
    /// <returns>
    /// The syntax error score for the navigation subsystem.
    /// </returns>
    public static int Compile(IList<string> lines)
    {
        var illegalCharacters = new List<char>();

        foreach (string line in lines)
        {
            bool isCorrupted = false;
            char illegal = default;
            var chunkStart = new Stack<char>();

            foreach (char value in line)
            {
                char expected = default;

                switch (value)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        chunkStart.Push(value);
                        continue;

                    case ')':
                        expected = '(';
                        break;

                    case ']':
                        expected = '[';
                        break;

                    case '}':
                        expected = '{';
                        break;

                    case '>':
                        expected = '<';
                        break;

                    default:
                        break;
                }

                if (chunkStart.Peek() != expected)
                {
                    illegal = value;
                    isCorrupted = true;
                    break;
                }

                chunkStart.Pop();
            }

            if (isCorrupted)
            {
                illegalCharacters.Add(illegal);
            }
        }

        var scores = new Dictionary<char, int>(4)
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };

        return illegalCharacters
            .Select((p) => scores[p])
            .Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> lines = await ReadResourceAsLinesAsync();

        SyntaxErrorScore = Compile(lines);

        if (Verbose)
        {
            Logger.WriteLine("The total syntax error score is {0:N0}.", SyntaxErrorScore);
        }

        return PuzzleResult.Create(SyntaxErrorScore);
    }
}
