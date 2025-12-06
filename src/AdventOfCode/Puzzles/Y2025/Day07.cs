// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 07, "", RequiresData = true, IsHidden = true, Unsolved = true)]
public sealed class Day07 : Puzzle<int>
{
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
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                int solution = Solve(values);

                if (logger is { })
                {
                    logger.WriteLine("The solution is {0}.", solution);
                }

                return solution;
            },
            cancellationToken);
    }
}
