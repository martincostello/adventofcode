// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 09, "Disk Fragmenter", RequiresData = true, IsHidden = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the checksum of the disk.
    /// </summary>
    public int Checksum { get; private set; }

    /// <summary>
    /// The disk map to defragment.
    /// </summary>
    /// <param name="map">The map of files and free space on the disk.</param>
    /// <returns>
    /// The checksum of the defragmented disk.
    /// </returns>
    public static int Defragment(ReadOnlySpan<char> map)
    {
        Debug.Assert(!map.IsEmpty, "No map provided.");
        return -1;
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
