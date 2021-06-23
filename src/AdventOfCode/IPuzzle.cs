// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// Defines a puzzle.
    /// </summary>
    public interface IPuzzle
    {
        /// <summary>
        /// Gets or sets a value indicating whether the puzzle should be run verbosely.
        /// </summary>
        bool Verbose { get; set; }

        /// <summary>
        /// Solves the puzzle given the specified input as an asynchronous operation.
        /// </summary>
        /// <param name="args">The input arguments to the puzzle.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous
        /// operation which returns the solution to the puzzle.
        /// </returns>
        Task<PuzzleResult> SolveAsync(string[] args, CancellationToken cancellationToken);
    }
}
