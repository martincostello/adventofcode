// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.Extensions.Caching.Memory;

namespace MartinCostello.AdventOfCode.Benchmarks;

////[EventPipeProfiler(EventPipeProfile.CpuSampling)] // Appears to hang when run via crank
[MemoryDiagnoser]
public class PuzzleBenchmarks
{
    public static IEnumerable<object> Puzzles()
    {
        foreach (object puzzle in Puzzles2015())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2016())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2017())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2018())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2019())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2020())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2021())
        {
            yield return puzzle;
        }

        foreach (object puzzle in Puzzles2022())
        {
            yield return puzzle;
        }
    }

    public static IEnumerable<object> Puzzles2015()
    {
        var inputs = new Dictionary<int, string[]>()
        {
            [4] = new[] { "iwrupvqb" },
            [10] = new[] { "1321131112" },
            [11] = new[] { "cqjxjnds" },
            [14] = new[] { "2503" },
            [17] = new[] { "150" },
            [20] = new[] { "34000000" },
            [25] = new[] { "2947", "3029" },
        };

        return GetPuzzles(2015, inputs);
    }

    public static IEnumerable<object> Puzzles2016()
    {
        var inputs = new Dictionary<int, string[]>()
        {
            [5] = new[] { "wtnhxymk" },
            [13] = new[] { "1362" },
            [16] = new[] { "10010000000110000", "272" },
            [17] = new[] { "pvhmgsws" },
            [19] = new[] { "3014387" },
            [21] = new[] { "abcdefgh" },
            [23] = new[] { "7" },
        };

        return GetPuzzles(2016, inputs);
    }

    public static IEnumerable<object> Puzzles2017()
    {
        var inputs = new Dictionary<int, string[]>()
        {
            [3] = new[] { "312051" },
            [14] = new[] { "hwlqcszp" },
        };

        return GetPuzzles(2017, inputs);
    }

    public static IEnumerable<object> Puzzles2018()
        => GetPuzzles(2018);

    public static IEnumerable<object> Puzzles2019()
    {
        var inputs = new Dictionary<int, string[]>()
        {
            [4] = new[] { "138241-674034" },
        };

        return GetPuzzles(2019, inputs);
    }

    public static IEnumerable<object> Puzzles2020()
    {
        var inputs = new Dictionary<int, string[]>()
        {
            [7] = new[] { "shiny gold" },
            [15] = new[] { "0,5,4,1,10,14,7" },
            [23] = new[] { "583976241" },
        };

        return GetPuzzles(2020, inputs);
    }

    public static IEnumerable<object> Puzzles2021()
        => GetPuzzles(2021);

    public static IEnumerable<object> Puzzles2022()
        => GetPuzzles(2022);

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2015))]
    public async Task<PuzzleResult> Solve2015(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2016))]
    public async Task<PuzzleResult> Solve2016(PuzzleInput input)
    => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2017))]
    public async Task<PuzzleResult> Solve2017(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2018))]
    public async Task<PuzzleResult> Solve2018(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2019))]
    public async Task<PuzzleResult> Solve2019(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2020))]
    public async Task<PuzzleResult> Solve2020(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2021))]
    public async Task<PuzzleResult> Solve2021(PuzzleInput input)
        => await input.SolveAsync();

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles2022))]
    public async Task<PuzzleResult> Solve2022(PuzzleInput input)
        => await input.SolveAsync();

    private static IEnumerable<object> GetPuzzles(
        int year,
        Dictionary<int, string[]>? inputs = null)
    {
        var puzzles = typeof(Puzzle).Assembly
            .GetTypes()
            .Where((p) => p.IsAssignableTo(typeof(Puzzle)))
            .Select((p) => new { Type = p, Metadata = p.GetCustomAttribute<PuzzleAttribute>()! })
            .Where((p) => p.Metadata is not null)
            .Where((p) => p.Metadata.Year == year)
            .Where((p) => !p.Metadata.IsHidden)
            .Where((p) => !p.Metadata.IsSlow)
            .OrderBy((p) => p.Metadata.Year)
            .OrderBy((p) => p.Metadata.Day)
            .ToList();

        var puzzleInput = typeof(PuzzleInput<>).GetGenericTypeDefinition();

        foreach (var puzzle in puzzles)
        {
            string[] input = Array.Empty<string>();

            if (inputs is not null &&
                inputs.TryGetValue(puzzle.Metadata.Day, out string[]? values))
            {
                input = values;
            }

            var puzzleInputOfT = puzzleInput.MakeGenericType(puzzle.Type);
            yield return Activator.CreateInstance(puzzleInputOfT, new object[] { input })!;
        }
    }

    public sealed class PuzzleInput<T> : PuzzleInput
        where T : IPuzzle, new()
    {
        public PuzzleInput(params string[] args)
            : base(args)
        {
            Puzzle = new T() { Verbose = false };

            if (Puzzle is Puzzle puzzle)
            {
                if (CacheResource)
                {
                    puzzle.Cache = InMemoryCache.Instance;
                }

                puzzle.Logger = NullLogger.Instance;
            }
        }

        public bool CacheResource { get; set; } = true;

        public override IPuzzle Puzzle { get; }

        private sealed class InMemoryCache : ICache, IDisposable
        {
            internal static readonly InMemoryCache Instance = new();
            private readonly MemoryCache _cache = new(new MemoryCacheOptions());

            public async Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> factory)
            {
                var result = await _cache.GetOrCreateAsync(key, (_) => factory());
                return result!;
            }

            public void Dispose() => _cache.Dispose();
        }

        private sealed class NullLogger : ILogger
        {
            internal static readonly NullLogger Instance = new();

            public string WriteGrid(bool[,] array, char falseChar, char trueChar)
                => string.Empty;

            public void WriteLine(string format, params object[] args)
            {
                // No-op
            }
        }
    }

    public abstract class PuzzleInput
    {
        protected PuzzleInput(params string[] args)
        {
            Args = args;
        }

        public string[] Args { get; }

        public abstract IPuzzle Puzzle { get; }

        public async Task<PuzzleResult> SolveAsync()
            => await Puzzle.SolveAsync(Args, CancellationToken.None);

        public override string ToString()
        {
            string[] split = Puzzle.GetType().FullName!.Split('.');

            string year = split[3];
            string day = split[4].Replace("Day", string.Empty, StringComparison.Ordinal);

            return $"{year}-{day}";
        }
    }
}
