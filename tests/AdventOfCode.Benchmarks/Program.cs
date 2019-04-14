// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Benchmarks
{
    using BenchmarkDotNet.Running;

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
        {
            var switcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);

            if (args?.Length == 0)
            {
                switcher.RunAll();
            }
            else
            {
                switcher.Run(args);
            }
        }
    }
}
