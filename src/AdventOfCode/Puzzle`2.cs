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
    /// Executes the specified solver using a single value from the specified arguments and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected PuzzleResult SolveWithArgument(
        IReadOnlyList<string> arguments,
        Func<string, ILogger?, (T1 Solution1, T2 Solution2)> solver)
    {
        (Solution1, Solution2) = solver(arguments[0], Verbose ? Logger : null);
        return PuzzleResult.Create(Solution1, Solution2);
    }

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
        Func<IReadOnlyList<string>, ILogger?, (T1 Solution1, T2 Solution2)> solver)
    {
        (Solution1, Solution2) = solver(arguments, Verbose ? Logger : null);
        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<IReadOnlyList<string>, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
    {
        (Solution1, Solution2) = solver(arguments, Verbose ? Logger : null, cancellationToken);
        return PuzzleResult.Create(Solution1, Solution2);
    }

    /// <summary>
    /// Executes the specified solver using a single value from the specified arguments and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithArgumentAsync(
        IReadOnlyList<string> arguments,
        Func<string, ILogger?, Task<(T1 Solution1, T2 Solution2)>> solver)
    {
        (Solution1, Solution2) = await solver(arguments[0], Verbose ? Logger : null);
        return PuzzleResult.Create(Solution1, Solution2);
    }

    /// <summary>
    /// Executes the specified solver using the specified arguments and returns the result.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithArgumentsAsync(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, ILogger?, Task<(T1 Solution1, T2 Solution2)>> solver)
    {
        (Solution1, Solution2) = await solver(arguments, Verbose ? Logger : null);
        return PuzzleResult.Create(Solution1, Solution2);
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
    protected async Task<PuzzleResult> SolveWithArgumentsAsync(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, ILogger?, CancellationToken, Task<(T1 Solution1, T2 Solution2)>> solver,
        CancellationToken cancellationToken)
    {
        (Solution1, Solution2) = await solver(arguments, Verbose ? Logger : null, cancellationToken);
        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<IReadOnlyList<string>, List<string>, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = solver(arguments, values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<List<string>, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<List<string>, ILogger?, CancellationToken, Task<(T1 Solution1, T2 Solution2)>> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = await solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<IReadOnlyList<string>, List<string>, ILogger?, CancellationToken, Task<(T1 Solution1, T2 Solution2)>> solver,
        CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = await solver(arguments, values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
    }

    /// <summary>
    /// Executes the specified solver using the input numbers from the resource and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="arguments">The arguments to pass to the solver.</param>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithNumbersAsync<T>(
        IReadOnlyList<string> arguments,
        Func<IReadOnlyList<string>, List<T>, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
        where T : INumber<T>
    {
        var values = await ReadResourceAsNumbersAsync<T>(cancellationToken);

        (Solution1, Solution2) = solver(arguments, values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
    }

    /// <summary>
    /// Executes the specified solver using the input numbers from the resource and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="solver">A delegate to a method that solves the puzzle.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="PuzzleResult"/> containing the solutions returned by <paramref name="solver"/>.
    /// </returns>
    protected async Task<PuzzleResult> SolveWithNumbersAsync<T>(
        Func<List<T>, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
        where T : INumber<T>
    {
        var values = await ReadResourceAsNumbersAsync<T>(cancellationToken);

        (Solution1, Solution2) = solver(values, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<string, ILogger?, CancellationToken, (T1 Solution1, T2 Solution2)> solver,
        CancellationToken cancellationToken)
    {
        string value = await ReadResourceAsStringAsync(cancellationToken);

        (Solution1, Solution2) = solver(value, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
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
        Func<string, ILogger?, CancellationToken, Task<(T1 Solution1, T2 Solution2)>> solver,
        CancellationToken cancellationToken)
    {
        string value = await ReadResourceAsStringAsync(cancellationToken);

        (Solution1, Solution2) = await solver(value, Verbose ? Logger : null, cancellationToken);

        return PuzzleResult.Create(Solution1, Solution2);
    }
}
