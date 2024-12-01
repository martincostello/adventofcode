// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 01, "Historian Hysteria", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the total distance between the values in the list.
    /// </summary>
    public int TotalDistance { get; private set; }

    /// <summary>
    /// Gets the similarity score between the values in the list.
    /// </summary>
    public int SimilarityScore { get; private set; }

    /// <summary>
    /// Gets the total distance between the values in the list.
    /// </summary>
    /// <param name="values">The list of values to get the total distance for.</param>
    /// <returns>
    /// The total distance between the values and the
    /// similarity score for the values in the list.
    /// </returns>
    public static (int TotalDistance, int SimilarityScore) ParseList(IList<string> values)
    {
        List<int> first = new(values.Count);
        List<int> second = new(values.Count);

        foreach (string pair in values)
        {
            string[] split = pair.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            first.Add(Parse<int>(split[0]));
            second.Add(Parse<int>(split[1]));
        }

        first.Sort();
        second.Sort();

        int total = 0;
        int similarity = 0;

        for (int i = 0; i < first.Count; i++)
        {
            int left = first[i];
            int right = second[i];

            total += Math.Abs(left - right);
            similarity += left * second.Count((p) => p == left);
        }

        return (total, similarity);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (TotalDistance, SimilarityScore) = ParseList(values);

        if (Verbose)
        {
            Logger.WriteLine("The total distance between the lists is {0}", TotalDistance);
            Logger.WriteLine("The similarity score for the lists is {0}", SimilarityScore);
        }

        return PuzzleResult.Create(TotalDistance, SimilarityScore);
    }
}
