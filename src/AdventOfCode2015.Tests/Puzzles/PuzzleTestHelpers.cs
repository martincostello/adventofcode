// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System.Globalization;
    using System.IO;
    using Xunit;

    /// <summary>
    /// A class containing methods for helping to test puzzles. This class cannot be inherited.
    /// </summary>
    internal static class PuzzleTestHelpers
    {
        /// <summary>
        /// Gets the path of the input file the specified day.
        /// </summary>
        /// <param name="day">The day of the puzzle to get the input path for.</param>
        /// <returns>
        /// The full path of the input file for the specified day.
        /// </returns>
        internal static string GetInputPath(int day)
        {
            string path = string.Format(
                CultureInfo.InvariantCulture,
                @".\Input\Day{0:00}\input.txt",
                day);

            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Solves the specified puzzle type with the specified arguments.
        /// </summary>
        /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <returns>
        /// The solved puzzle of the type specified by <typeparamref name="T"/>.
        /// </returns>
        internal static T SolvePuzzle<T>(string[] args)
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
