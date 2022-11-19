// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 01, "", RequiresData = true, IsHidden = true)]
public sealed class Day01 : Puzzle
{
    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);
        return await Task.FromResult(PuzzleResult.Create());
    }
}
