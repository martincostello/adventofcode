// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// The base class for puzzles with one solution.
/// </summary>
/// <typeparam name="T">The type of the solution.</typeparam>
public abstract class Puzzle<T> : Puzzle
    where T : notnull
{
    /// <summary>
    /// Gets or sets the solution.
    /// </summary>
    public T Solution { get; protected set; } = default!;

    /// <summary>
    /// Executes the specified solver using the specified arguments and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected PuzzleResult SolveWithArguments(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, ILogger?, T> solver)
    {
        Solution = solver(arguments, Verbose ? Logger : null);
        return PuzzleResult.Create(Solution);
    }

    /// <summary>
    /// Executes the specified solver using the specified arguments and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected PuzzleResult SolveWithArguments(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, ILogger?, CancellationToken, T> solver,
        CancellationToken cancellationToken)
    {
        Solution = solver(arguments, Verbose ? Logger : null, cancellationToken);
        return PuzzleResult.Create(Solution);
    }

    /// <summary>
    /// Executes the specified solver using the input lines from the resource and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithLinesAsync(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, List<string>, ILogger?, CancellationToken, T> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Solution = solver(arguments, values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution);
    }

    /// <summary>
    /// Executes the specified solver using the input lines from the resource and returns the result.
    /// </summary>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithLinesAsync(
        Func<List<string>, ILogger?, CancellationToken, T> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Solution = solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution);
    }

    /// <summary>
    /// Executes the specified solver using the input lines from the resource and returns the result.
    /// </summary>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithLinesAsync(
        Func<List<string>, ILogger?, CancellationToken, Task<T>> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Solution = await solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution);
    }

    /// <summary>
    /// Executes the specified solver using the input string from the resource and returns the result.
    /// </summary>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithStringAsync(
        Func<string, ILogger?, CancellationToken, Task<T>> solver,
        CancellationToken cancellationToken)
    {
        string value = await ReadResourceAsStringAsync(cancellationToken);

        Solution = await solver(value, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution);
    }
}
