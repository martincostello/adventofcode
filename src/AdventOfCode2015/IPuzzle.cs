// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    /// <summary>
    /// Defines a puzzle.
    /// </summary>
    internal interface IPuzzle
    {
        /// <summary>
        /// Solves the puzzle given the specified input.
        /// </summary>
        /// <param name="args">The input arguments to the puzzle.</param>
        /// <returns>The exit code the application should return.</returns>
        int Solve(string[] args);
    }
}
