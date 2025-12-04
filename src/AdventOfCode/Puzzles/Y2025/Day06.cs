// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 06, "", RequiresData = true, IsHidden = true, Unsolved = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the solution.
    /// </summary>
    public int Solution { get; private set; }

    /// <summary>
    /// Solves the puzzle.
    /// </summary>
    /// <param name="values">The values to solve the puzzle from.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static int Solve(IReadOnlyList<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return Unsolved;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Solution = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("The solution is {0}.", Solution);
        }

        return PuzzleResult.Create(Solution);
    }
}
