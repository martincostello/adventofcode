// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 14, RequiresData = true)]
public sealed class Day14 : Puzzle
{
    /// <summary>
    /// Gets the "score" of the polymer after being expanded 10 times,
    /// which is the quantity of the most common element subtracted by
    /// the quantity of the least common element.
    /// </summary>
    public long Score { get; private set; }

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

        var pairs = new Dictionary<string, char>(instructions.Count - 2);

        foreach (string instruction in instructions.Skip(2))
        {
            string[] split = instruction.Split(" -> ");
            pairs[split[0]] = split[1][0];
        }

        var frequencies = template
            .GroupBy((p) => p)
            .Select((p) => new
            {
                Letter = p.Key,
                Count = p.Count(),
            })
            .ToDictionary((p) => p.Letter, (p) => p.Count);

        var current = new StringBuilder(template);
        StringBuilder next;

        for (int i = 0; i < steps; i++)
        {
            string previous = current.ToString();
            next = new StringBuilder(previous);

            for (int j = 0, k = 0; j < previous.Length - 1; j++)
            {
                string pair = new(new[] { previous[j], previous[j + 1] });

                if (pairs.TryGetValue(pair, out char element))
                {
                    next.Insert(j + k++ + 1, element);
                    frequencies.AddOrIncrement(element, 1);
                }
            }

            current = next;
        }

        int max = frequencies.Values.Max();
        int min = frequencies.Values.Min();

        return max - min;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        Score = Expand(instructions, steps: 10);

        if (Verbose)
        {
            Logger.WriteLine(
                "The \"score\" of the polymer after 10 steps of expansion is {0:N0}.",
                Score);
        }

        return PuzzleResult.Create(Score);
    }
}
