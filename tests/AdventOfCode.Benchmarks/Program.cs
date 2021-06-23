// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Running;

namespace MartinCostello.AdventOfCode.Benchmarks
{
    /// <summary>
    /// A console application that runs performance benchmarks for the puzzles. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        internal static void Main(string[] args)
            => BenchmarkRunner.Run<PuzzleBenchmarks>(args: args);
    }
}
