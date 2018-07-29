// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Benchmarks
{
    using System;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser]
    public abstract class PuzzleBenchmark<T>
        where T : IPuzzle, new()
    {
        private readonly T _puzzle = new T();

        public string[] Arguments { get; set; } = Array.Empty<string>();

        [Benchmark]
        public int Solve() => _puzzle.Solve(Arguments);
    }
}
