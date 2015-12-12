// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using System;

    /// <summary>
    /// A class representing an unimplemented puzzle.
    /// </summary>
    internal class NotImplementedPuzzle : IPuzzle
    {
        /// <inheritdoc />
        public int Solve(string[] args)
        {
            Console.Error.WriteLine("This puzzle is not implemented yet.");
            return -1;
        }
    }
}
