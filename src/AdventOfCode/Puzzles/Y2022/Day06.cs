// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 06, "Tuning Trouble", RequiresData = true)]
public sealed class Day06 : Puzzle<int, int>
{
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

        foreach ((int index, char item) in datastream.Index())
        {
            if (queue.Count == distinctCharacters &&
                queue.Distinct().Count() == distinctCharacters)
            {
                return index;
            }

            queue.Enqueue(item);

            if (queue.Count > distinctCharacters)
            {
                _ = queue.Dequeue();
            }
        }

        return 0;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithStringAsync(
            static (datastream, logger) =>
            {
                int indexOfFirstStartOfPacketMarker = FindFirstPacket(datastream, 4);
                int indexOfFirstStartOfMessageMarker = FindFirstPacket(datastream, 14);

                if (logger is { })
                {
                    logger.WriteLine(
                        "{0} characters need to be processed before the first start-of-packet marker is detected.",
                        indexOfFirstStartOfPacketMarker);

                    logger.WriteLine(
                        "{0} characters need to be processed before the first start-of-message marker is detected.",
                        indexOfFirstStartOfMessageMarker);
                }

                return (indexOfFirstStartOfPacketMarker, indexOfFirstStartOfMessageMarker);
            },
            cancellationToken);
    }
}
