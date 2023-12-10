// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/09</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 09, "Mirage Maintenance", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the sum of these extrapolated values.
    /// </summary>
    public int Sum { get; private set; }

    /// <summary>
    /// Analyze the OASIS report and extrapolate the next value for each history.
    /// </summary>
    /// <param name="histories">The histories to analyze.</param>
    /// <returns>
    /// The sum of these extrapolated values.
    /// </returns>
    public static int Analyze(IList<string> histories)
    {
        var extrapolated = new List<int>();

        foreach (string history in histories)
        {
            var sequence = history.Split(' ').Select(Parse<int>).ToList();
            var sequences = new List<List<int>>() { sequence };

            while (!sequence.All((p) => p is 0))
            {
                sequence = sequence.Pairwise((x, y) => y - x).ToList();
                sequences.Add(sequence);
            }

            sequence.Add(0);

            for (int i = sequences.Count - 2; i > -1; i--)
            {
                var current = sequences[i];
                var previous = sequences[i + 1];

                int next = previous[^1] + current[^1];

                current.Add(next);
            }

            extrapolated.Add(sequences[0][^1]);
        }

        return extrapolated.Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Sum = Analyze(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of these extrapolated values is {0}.", Sum);
        }

        return PuzzleResult.Create(Sum);
    }
}
