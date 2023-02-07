// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 06, "Tuning Trouble", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the index of the first start-of-packet marker.
    /// </summary>
    public int IndexOfFirstStartOfPacketMarker { get; private set; }

    /// <summary>
    /// Gets the index of the first start-of-message marker.
    /// </summary>
    public int IndexOfFirstStartOfMessageMarker { get; private set; }

    /// <summary>
    /// Finds the index of the first start-of-packet marker in the datastream.
    /// </summary>
    /// <param name="datastream">The datastream to find the index of the first start-of-packet marker.</param>
    /// <param name="distinctCharacters">The number of distinct characters required to find the marker.</param>
    /// <returns>
    /// The index of the first start-of-packet marker.
    /// </returns>
    public static int FindFirstPacket(string datastream, int distinctCharacters)
    {
        var queue = new Queue<char>(distinctCharacters);
        int index = 0;

        foreach (char item in datastream)
        {
            if (queue.Count == distinctCharacters &&
                queue.Distinct().Count() == distinctCharacters)
            {
                break;
            }

            index++;

            queue.Enqueue(item);

            if (queue.Count > distinctCharacters)
            {
                _ = queue.Dequeue();
            }
        }

        return index;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string datastream = await ReadResourceAsStringAsync(cancellationToken);

        IndexOfFirstStartOfPacketMarker = FindFirstPacket(datastream, 4);
        IndexOfFirstStartOfMessageMarker = FindFirstPacket(datastream, 14);

        if (Verbose)
        {
            Logger.WriteLine(
                "{0} characters need to be processed before the first start-of-packet marker is detected.",
                IndexOfFirstStartOfPacketMarker);

            Logger.WriteLine(
                "{0} characters need to be processed before the first start-of-message marker is detected.",
                IndexOfFirstStartOfMessageMarker);
        }

        return PuzzleResult.Create(IndexOfFirstStartOfPacketMarker, IndexOfFirstStartOfMessageMarker);
    }
}
