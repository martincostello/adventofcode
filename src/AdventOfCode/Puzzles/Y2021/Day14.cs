// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 14, "Extended Polymerization", RequiresData = true)]
public sealed class Day14 : Puzzle
{
    /// <summary>
    /// Gets the "score" of the polymer after being expanded 10 times,
    /// which is the quantity of the most common element subtracted by
    /// the quantity of the least common element.
    /// </summary>
    public long Score10 { get; private set; }

    /// <summary>
    /// Gets the "score" of the polymer after being expanded 40 times,
    /// which is the quantity of the most common element subtracted by
    /// the quantity of the least common element.
    /// </summary>
    public long Score40 { get; private set; }

    /// <summary>
    /// Expands the polymer template as specified by the instructions.
    /// </summary>
    /// <param name="instructions">The instructions to expand a polymer from.</param>
    /// <param name="steps">The number of steps to expand the polymer by.</param>
    /// <returns>
    /// The polymer's score after expansion.
    /// </returns>
    public static long Expand(IList<string> instructions, int steps)
    {
        string template = instructions[0];

        var insertions = new Dictionary<string, char>(instructions.Count - 2);

        foreach (string instruction in instructions.Skip(2))
        {
            string[] split = instruction.Split(" -> ");
            string pair = split[0];
            char element = split[1][0];

            insertions[pair] = element;
        }

        var pairCounts = new Dictionary<string, long>();

        for (int i = 0; i < template.Length - 1; i++)
        {
            char first = template[i];
            char second = template[i + 1];

            string pair = first + string.Empty + second;

            pairCounts.AddOrIncrement(pair, 1);
        }

        for (int i = 1; i <= steps; i++)
        {
            foreach ((string pair, long count) in pairCounts.ToArray())
            {
                if (count > 0 && insertions.TryGetValue(pair, out char element))
                {
                    pairCounts[pair] -= count;

                    string pair1 = pair[0] + string.Empty + element;
                    string pair2 = element + string.Empty + pair[1];

                    pairCounts.AddOrIncrement(pair1, count, count);
                    pairCounts.AddOrIncrement(pair2, count, count);
                }
            }
        }

        var frequencies = new Dictionary<char, long>();

        foreach ((string pair, long count) in pairCounts)
        {
            frequencies.AddOrIncrement(pair[0], count, count);
            frequencies.AddOrIncrement(pair[1], count, count);
        }

        long max = frequencies.Values.Max();
        long min = frequencies.Values.Min();

        return (max - min + 1) / 2;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync(cancellationToken);

        Score10 = Expand(instructions, steps: 10);
        Score40 = Expand(instructions, steps: 40);

        if (Verbose)
        {
            Logger.WriteLine("The \"score\" of the polymer after 10 steps of expansion is {0:N0}.", Score10);
            Logger.WriteLine("The \"score\" of the polymer after 40 steps of expansion is {0:N0}.", Score40);
        }

        return PuzzleResult.Create(Score10, Score40);
    }
}
