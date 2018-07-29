// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using System;
    using Xunit;

    /// <summary>
    /// A class containing methods for helping to test puzzles. This class cannot be inherited.
    /// </summary>
    internal static class PuzzleTestHelpers
    {
        /// <summary>
        /// Solves the specified puzzle type.
        /// </summary>
        /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
        /// <returns>
        /// The solved puzzle of the type specified by <typeparamref name="T"/>.
        /// </returns>
        internal static T SolvePuzzle<T>()
            where T : IPuzzle, new()
        {
            return SolvePuzzle<T>(Array.Empty<string>());
        }

        /// <summary>
        /// Solves the specified puzzle type with the specified arguments.
        /// </summary>
        /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <returns>
        /// The solved puzzle of the type specified by <typeparamref name="T"/>.
        /// </returns>
        internal static T SolvePuzzle<T>(params string[] args)
            where T : IPuzzle, new()
        {
            // Arrange
            T puzzle = new T();

            // Act
            int result = puzzle.Solve(args);

            // Assert
            Assert.Equal(0, result);

            return puzzle;
        }
    }
}
