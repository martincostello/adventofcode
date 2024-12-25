﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 23, "LAN Party", RequiresData = true, IsHidden = true)]
public sealed class Day23 : Puzzle
{
    /// <summary>
    /// Gets the number of networks that contain at least one computer with a name that starts with <c>t</c>.
    /// </summary>
    public int TNetworkCount { get; private set; }

    /// <summary>
    /// Counts the number of networks that contain at least
    /// one computer with a name that starts with <c>t</c>.
    /// </summary>
    /// <param name="values">A list of connections in the network(s).</param>
    /// <returns>
    /// The number of networks that contain at least one computer with a name that starts with <c>t</c>.
    /// </returns>
    public static int CountNetworks(IList<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return -1;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        TNetworkCount = CountNetworks(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} networks contain at least one computer with a name that starts with t.", TNetworkCount);
        }

        return PuzzleResult.Create(TNetworkCount);
    }
}
