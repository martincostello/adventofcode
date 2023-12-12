// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 12, "Hot Springs", RequiresData = true)]
public sealed class Day12 : Puzzle
{
    private const char Damaged = '#';
    private const char Unknown = '?';

    /// <summary>
    /// Gets the sum of the counts of the possible spring arrangements.
    /// </summary>
    public int SumOfCounts { get; private set; }

    /// <summary>
    /// Analyzes the specified spring arrangement.
    /// </summary>
    /// <param name="record">The spring arrangement to analyze.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The counts of possible spring arrangements.
    /// </returns>
    public static int Analyze(string record, CancellationToken cancellationToken)
    {
        (string values, string countList) = record.Bifurcate(' ');

        int[] counts = countList.Split(',').Select(Parse<int>).ToArray();
        int actual = values.Count(Damaged);
        int desired = counts.Sum();

        var spring = new PartialSpringRecord();
        var steps = PathFinding.BreadthFirst(spring, new State(values, counts, actual, desired), cancellationToken);

        return steps.Count((p) => p.IsValid());
    }

    /// <summary>
    /// Analyzes the specified spring arrangements.
    /// </summary>
    /// <param name="records">The spring arrangements to analyze.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The sum of the counts of the possible spring arrangements.
    /// </returns>
    public static int Analyze(IList<string> records, CancellationToken cancellationToken)
        => records.Select((p) => Analyze(p, cancellationToken)).Sum();

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var records = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfCounts = Analyze(records, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the counts of spring arrangements is {0}.", SumOfCounts);
        }

        return PuzzleResult.Create(SumOfCounts);
    }

    private record struct State(string Values, int[] Counts, int Actual, int Desired)
    {
        public readonly bool IsValid()
        {
            if (Actual != Desired)
            {
                return false;
            }

            var window = Values.AsSpan();
            int index = window.IndexOf(Damaged);

            if (index < 0)
            {
                return false;
            }

            window = window[index..];

            int groups = 0;

            for (int i = 0; i < Counts.Length; i++)
            {
                int desired = Counts[i];
                int count = 0;

                while (!window.IsEmpty && window[0] is Damaged)
                {
                    count++;
                    window = window[1..];
                }

                if (count != desired)
                {
                    break;
                }

                groups++;

                while (!window.IsEmpty && window[0] is not Damaged)
                {
                    window = window[1..];
                }

                if (window.IsEmpty)
                {
                    break;
                }
            }

            return groups == Counts.Length;
        }
    }

    private sealed class PartialSpringRecord : IGraph<State>
    {
        public IEnumerable<State> Neighbors(State id)
        {
            if (id.Actual == id.Desired)
            {
                // No more springs to find
                yield break;
            }

            string springs = id.Values;

            for (int i = springs.IndexOf(Unknown, StringComparison.Ordinal); i > -1;)
            {
                string next = string.Create(springs.Length, (id.Values, i), (span, state) =>
                {
                    for (int j = 0; j < span.Length; j++)
                    {
                        span[j] = j == state.i ? Damaged : state.Values[j];
                    }
                });

                yield return new State(next, id.Counts, id.Actual + 1, id.Desired);

                i = springs.IndexOf(Unknown, i + 1);
            }
        }
    }
}
