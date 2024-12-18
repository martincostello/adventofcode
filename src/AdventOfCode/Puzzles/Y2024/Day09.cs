// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 09, "Disk Fragmenter", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the checksum of the defragmented disk.
    /// </summary>
    public long Checksum { get; private set; }

    /// <summary>
    /// Defragments the specified disk.
    /// </summary>
    /// <param name="map">The map for the disk of files and free space.</param>
    /// <returns>
    /// The checksum of the defragmented disk.
    /// </returns>
    public static long Defragment(ReadOnlySpan<char> map)
    {
        var blocks = new List<(int Id, int Index, int Length)>();
        var spaces = new List<(int Index, int Length)>();

        int id = 0;
        int offset = 0;

        for (int i = 0; i < map.Length; i++)
        {
            int length = map[i] - '0';

            if (i % 2 is 0)
            {
                blocks.Add((id++, offset, length));
            }
            else
            {
                spaces.Add((offset, length));
            }

            offset += length;
        }

        spaces.Reverse();

        var availableSpace = new Stack<(int Index, int Length)>(spaces);
        var defragmented = new List<(int Id, int Index, int Length)>();

        while (availableSpace.Count > 0)
        {
            var space = availableSpace.Pop();

            (id, offset, int length) = blocks[^1];

            if (offset + length < space.Index)
            {
                // The next file is before the first remaining free space, so we're done
                break;
            }

            blocks.RemoveAt(blocks.Count - 1);

            if (length > space.Length)
            {
                blocks.Add((id, offset, length - space.Length));
                defragmented.Add((id, space.Index, space.Length));
            }
            else if (length == space.Length)
            {
                defragmented.Add((id, space.Index, space.Length));
            }
            else
            {
                // Space bigger than required
                defragmented.Add((id, space.Index, length));
                availableSpace.Push((space.Index + length, space.Length - length));
            }
        }

        long checksum = 0;

        foreach (var block in blocks)
        {
            for (int i = 0; i < block.Length; i++)
            {
                checksum += (block.Index + i) * block.Id;
            }
        }

        foreach (var block in defragmented)
        {
            for (int i = 0; i < block.Length; i++)
            {
                checksum += (block.Index + i) * block.Id;
            }
        }

        return checksum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string map = await ReadResourceAsStringAsync(cancellationToken);

        Checksum = Defragment(map);

        if (Verbose)
        {
            Logger.WriteLine("The filesystem checksum is {0}.", Checksum);
        }

        return PuzzleResult.Create(Checksum);
    }
}
