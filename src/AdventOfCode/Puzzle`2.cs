// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// The base class for puzzles with two solutions.
/// </summary>
/// <typeparam name="T1">The type of the solution to part 1.</typeparam>
/// <typeparam name="T2">The type of the solution to part 2.</typeparam>
public abstract class Puzzle<T1, T2> : Puzzle
    where T1 : notnull
    where T2 : notnull
{
    /// <summary>
    /// Gets or sets the solution to part 1.
    /// </summary>
    public T1 Solution1 { get; protected set; } = default!;

    /// <summary>
    /// Gets or sets the solution to part 2.
    /// </summary>
    public T2 Solution2 { get; protected set; } = default!;

    /// <summary>
    /// Creates a new <see cref="PuzzleResult"/> instance using the current solutions.
    /// </summary>
    /// <returns>
    /// A <see cref="PuzzleResult"/> for <see cref="Solution1"/> and <see cref="Solution2"/>.
    /// </returns>
    protected PuzzleResult Result() => PuzzleResult.Create(Solution1, Solution2);

    /// <summary>
    /// Executes the specified solver using the input lines from the resource and returns the result.
    /// </summary>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithLinesAsync(
        Func<List<string>, ILogger?, CancellationToken, Task<(T1 Solution1, T2 Solution2)>> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = await solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
    }
}
