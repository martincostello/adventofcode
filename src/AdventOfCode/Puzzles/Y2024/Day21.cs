﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 21, "Keypad Conundrum", RequiresData = true, IsHidden = true)]
public sealed class Day21 : Puzzle
{
#pragma warning disable IDE0022
#pragma warning disable IDE0060
#pragma warning disable SA1600

    public int Solution { get; private set; }

    public static int Solve(IList<string> values)
    {
        return -1;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Solution = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("{0}", Solution);
        }

        return PuzzleResult.Create(Solution);
    }
}
