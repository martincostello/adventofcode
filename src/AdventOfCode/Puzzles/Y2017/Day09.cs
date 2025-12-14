// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 09, "Stream Processing", RequiresData = true)]
public sealed class Day09 : Puzzle<int, int>
{
    /// <summary>
    /// Computes the total score for all the groups in the specified stream.
    /// </summary>
    /// <param name="stream">The stream to get the score for.</param>
    /// <returns>
    /// The total score for the groups in the stream specified by <paramref name="stream"/>.
    /// </returns>
    public static (int Score, int GarbageCount) ParseStream(ReadOnlySpan<char> stream)
    {
        int score = 0;
        int level = 0;
        int garbageCount = 0;
        bool withinGarbage = false;

        for (int i = 0; i < stream.Length; i++)
        {
            char c = stream[i];

            switch (c)
            {
                case '{':
                    if (withinGarbage)
                    {
                        garbageCount++;
                    }
                    else
                    {
                        level++;
                        score += level;
                    }

                    break;

                case '}':
                    if (withinGarbage)
                    {
                        garbageCount++;
                    }
                    else
                    {
                        level--;
                    }

                    break;

                case '<':
                    if (withinGarbage)
                    {
                        garbageCount++;
                    }
                    else
                    {
                        withinGarbage = true;
                    }

                    break;

                case '>':
                    withinGarbage = false;
                    break;

                case '!':
                    i++;
                    break;

                case ',':
                default:
                    if (withinGarbage)
                    {
                        garbageCount++;
                    }

                    break;
            }
        }

        return (score, garbageCount);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithStringAsync(
            static (stream, logger, _) =>
            {
                stream = stream.Trim();

                (int totalScore, int garbageCount) = ParseStream(stream);

                if (logger is { })
                {
                    logger.WriteLine($"The total score for all the groups is {totalScore:N0}.");
                    logger.WriteLine($"There are {garbageCount:N0} non-canceled characters within the garbage.");
                }

                return (totalScore, garbageCount);
            },
            cancellationToken);
    }
}
