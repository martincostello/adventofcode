// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the result of solving a puzzle. This class cannot be inherited.
    /// </summary>
    public sealed class PuzzleResult
    {
        /// <summary>
        /// Gets the solution(s) to the puzzle.
        /// </summary>
        public IList<object> Solutions { get; private set; } = new List<object>();

        /// <summary>
        /// Gets the sisualization(s) associated with the puzzle, if any.
        /// </summary>
        public IList<string> Visualizations { get; private set; } = new List<string>();

        /// <summary>
        /// Converts a puzzle result to a <see cref="Task{PuzzleResult}"/>.
        /// </summary>
        /// <param name="value">The puzzle result to convert.</param>
        /// <returns>
        /// A <see cref="Task{PuzzleResult}"/> for <paramref name="value"/>.
        /// </returns>
        public static implicit operator Task<PuzzleResult>(PuzzleResult value)
            => Task.FromResult(value);

        /// <summary>
        /// Creates a <see cref="Task{PuzzleResult}"/> from the specified solution(s).
        /// </summary>
        /// <param name="solutions">The solution(s) to the puzzle.</param>
        /// <returns>
        /// A <see cref="PuzzleResult"/> for <paramref name="solutions"/>.
        /// </returns>
        public static PuzzleResult Create(params object[] solutions)
            => new PuzzleResult() { Solutions = solutions };
    }
}
