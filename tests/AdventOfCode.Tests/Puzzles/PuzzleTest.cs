// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles;

/// <summary>
/// The class class for puzzle tests.
/// </summary>
public abstract class PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleTest"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    protected PuzzleTest(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        Logger = new TestLogger(OutputHelper);
    }

    /// <summary>
    /// Gets the <see cref="ITestOutputHelper"/> to use.
    /// </summary>
    protected ITestOutputHelper OutputHelper { get; }

    /// <summary>
    /// Gets the <see cref="ILogger"/> to use.
    /// </summary>
    private protected ILogger Logger { get; }

    /// <summary>
    /// Solves the specified puzzle type asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
    /// <returns>
    /// The solved puzzle of the type specified by <typeparamref name="T"/>.
    /// </returns>
    protected async Task<T> SolvePuzzleAsync<T>()
        where T : Puzzle, new()
        => await SolvePuzzleAsync<T>([]);

    /// <summary>
    /// Solves the specified puzzle type with the specified arguments asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
    /// <param name="args">The arguments to pass to the puzzle.</param>
    /// <returns>
    /// The solved puzzle of the type specified by <typeparamref name="T"/>.
    /// </returns>
    protected async Task<T> SolvePuzzleAsync<T>(params string[] args)
        where T : Puzzle, new()
    {
        // Arrange
        var puzzle = new T()
        {
            Logger = Logger,
            Verbose = true,
        };

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

        // Act
        PuzzleResult result = await puzzle.SolveAsync(args, cts.Token);

        // Assert
        result.ShouldNotBeNull();
        result.Solutions.ShouldNotBeNull();
        result.Solutions.Count.ShouldBeGreaterThan(0);

        return puzzle;
    }
}
