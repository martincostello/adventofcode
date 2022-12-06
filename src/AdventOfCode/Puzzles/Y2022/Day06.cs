// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 06, "CHANGE_ME", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the index of the first start-of-packet marker.
    /// </summary>
    public int IndexOfFirstStartOfPacketMarker { get; private set; }

    /// <summary>
    /// Finds the index of the first start-of-packet marker in the datastream.
    /// </summary>
    /// <param name="datastream">The datastream to find the index of the first start-of-packet marker.</param>
    /// <returns>
    /// The index of the first start-of-packet marker.
    /// </returns>
    public static int FindFirstPacket(string datastream)
    {
        var queue = new Queue<char>(4);
        int result = 0;

        foreach (char item in datastream)
        {
            if (queue.Count == 4 && queue.Distinct().Count() == 4)
            {
                break;
            }

            result++;

            queue.Enqueue(item);

            if (queue.Count > 4)
            {
                _ = queue.Dequeue();
            }
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string datastream = await ReadResourceAsStringAsync();

        IndexOfFirstStartOfPacketMarker = FindFirstPacket(datastream);

        if (Verbose)
        {
            Logger.WriteLine(
                "{0} characters need to be processed before the first start-of-packet marker is detected",
                IndexOfFirstStartOfPacketMarker);
        }

        return PuzzleResult.Create(IndexOfFirstStartOfPacketMarker);
    }
}
