// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace MartinCostello.AdventOfCode.Benchmarks;

[EventPipeProfiler(EventPipeProfile.CpuSampling)]
[MemoryDiagnoser]
public class PuzzleBenchmarks
{
    public static IEnumerable<object> Puzzles()
    {
        // Classes not benchmarked are either too slow or not implemented
        yield return new PuzzleInput<Puzzles.Y2015.Day01>();
        yield return new PuzzleInput<Puzzles.Y2015.Day02>();
        yield return new PuzzleInput<Puzzles.Y2015.Day03>();
        yield return new PuzzleInput<Puzzles.Y2015.Day04>("iwrupvqb", "5");
        yield return new PuzzleInput<Puzzles.Y2015.Day05>("1");
        yield return new PuzzleInput<Puzzles.Y2015.Day06>("1");
        yield return new PuzzleInput<Puzzles.Y2015.Day07>();
        yield return new PuzzleInput<Puzzles.Y2015.Day08>();
        yield return new PuzzleInput<Puzzles.Y2015.Day09>();
        yield return new PuzzleInput<Puzzles.Y2015.Day11>("cqjxjnds");
        yield return new PuzzleInput<Puzzles.Y2015.Day12>();
        yield return new PuzzleInput<Puzzles.Y2015.Day14>("2503");
        yield return new PuzzleInput<Puzzles.Y2015.Day16>();
        yield return new PuzzleInput<Puzzles.Y2015.Day17>("150");
        yield return new PuzzleInput<Puzzles.Y2015.Day18>("100", "false");
        yield return new PuzzleInput<Puzzles.Y2015.Day19>("calibrate");
        yield return new PuzzleInput<Puzzles.Y2015.Day21>();
        yield return new PuzzleInput<Puzzles.Y2015.Day23>();
        yield return new PuzzleInput<Puzzles.Y2015.Day25>("2947", "3029");
        yield return new PuzzleInput<Puzzles.Y2016.Day01>();
        yield return new PuzzleInput<Puzzles.Y2016.Day02>();
        yield return new PuzzleInput<Puzzles.Y2016.Day03>();
        yield return new PuzzleInput<Puzzles.Y2016.Day04>();
        yield return new PuzzleInput<Puzzles.Y2016.Day06>();
        yield return new PuzzleInput<Puzzles.Y2016.Day07>();
        yield return new PuzzleInput<Puzzles.Y2016.Day08>();
        yield return new PuzzleInput<Puzzles.Y2016.Day09>();
        yield return new PuzzleInput<Puzzles.Y2016.Day10>();
        yield return new PuzzleInput<Puzzles.Y2016.Day13>("1362");
        yield return new PuzzleInput<Puzzles.Y2016.Day15>();
        yield return new PuzzleInput<Puzzles.Y2016.Day16>("10010000000110000", "272");
        yield return new PuzzleInput<Puzzles.Y2016.Day17>("pvhmgsws");
        yield return new PuzzleInput<Puzzles.Y2016.Day18>("40");
        yield return new PuzzleInput<Puzzles.Y2016.Day19>("3014387", "1");
        yield return new PuzzleInput<Puzzles.Y2016.Day19>("3014387", "2");
        yield return new PuzzleInput<Puzzles.Y2016.Day20>();
        yield return new PuzzleInput<Puzzles.Y2016.Day21>("abcdefgh");
        yield return new PuzzleInput<Puzzles.Y2016.Day22>();
        yield return new PuzzleInput<Puzzles.Y2016.Day23>("7");
        yield return new PuzzleInput<Puzzles.Y2016.Day24>();
        yield return new PuzzleInput<Puzzles.Y2017.Day01>();
        yield return new PuzzleInput<Puzzles.Y2017.Day02>();
        yield return new PuzzleInput<Puzzles.Y2017.Day03>("312051");
        yield return new PuzzleInput<Puzzles.Y2017.Day04>();
        yield return new PuzzleInput<Puzzles.Y2017.Day05>();
        yield return new PuzzleInput<Puzzles.Y2017.Day06>();
        yield return new PuzzleInput<Puzzles.Y2017.Day07>();
        yield return new PuzzleInput<Puzzles.Y2017.Day08>();
        yield return new PuzzleInput<Puzzles.Y2017.Day09>();
        yield return new PuzzleInput<Puzzles.Y2017.Day10>();
        yield return new PuzzleInput<Puzzles.Y2017.Day11>();
        yield return new PuzzleInput<Puzzles.Y2017.Day12>();
        yield return new PuzzleInput<Puzzles.Y2017.Day13>();
        yield return new PuzzleInput<Puzzles.Y2017.Day14>("hwlqcszp");
        yield return new PuzzleInput<Puzzles.Y2018.Day02>();
        yield return new PuzzleInput<Puzzles.Y2018.Day03>("312051");
        yield return new PuzzleInput<Puzzles.Y2018.Day04>();
        yield return new PuzzleInput<Puzzles.Y2018.Day06>();
        yield return new PuzzleInput<Puzzles.Y2018.Day07>();
        yield return new PuzzleInput<Puzzles.Y2018.Day08>();
        yield return new PuzzleInput<Puzzles.Y2019.Day01>();
        yield return new PuzzleInput<Puzzles.Y2019.Day02>();
        yield return new PuzzleInput<Puzzles.Y2019.Day03>();
        yield return new PuzzleInput<Puzzles.Y2019.Day04>("138241-674034");
        yield return new PuzzleInput<Puzzles.Y2019.Day05>("1");
        yield return new PuzzleInput<Puzzles.Y2019.Day07>();
        yield return new PuzzleInput<Puzzles.Y2019.Day08>();
        yield return new PuzzleInput<Puzzles.Y2019.Day09>("1");
        yield return new PuzzleInput<Puzzles.Y2020.Day01>();
        yield return new PuzzleInput<Puzzles.Y2020.Day02>();
        yield return new PuzzleInput<Puzzles.Y2020.Day03>();
        yield return new PuzzleInput<Puzzles.Y2020.Day04>();
        yield return new PuzzleInput<Puzzles.Y2020.Day05>();
        yield return new PuzzleInput<Puzzles.Y2020.Day06>();
        yield return new PuzzleInput<Puzzles.Y2020.Day07>("shiny gold");
        yield return new PuzzleInput<Puzzles.Y2020.Day08>();
        yield return new PuzzleInput<Puzzles.Y2020.Day09>();
        yield return new PuzzleInput<Puzzles.Y2020.Day10>();
        yield return new PuzzleInput<Puzzles.Y2020.Day11>();
        yield return new PuzzleInput<Puzzles.Y2020.Day12>();
        yield return new PuzzleInput<Puzzles.Y2020.Day13>();
        yield return new PuzzleInput<Puzzles.Y2020.Day14>();
        yield return new PuzzleInput<Puzzles.Y2020.Day16>();
        yield return new PuzzleInput<Puzzles.Y2020.Day18>();
        yield return new PuzzleInput<Puzzles.Y2020.Day19>();
        yield return new PuzzleInput<Puzzles.Y2020.Day20>();
        yield return new PuzzleInput<Puzzles.Y2020.Day21>();
        yield return new PuzzleInput<Puzzles.Y2020.Day22>();
        yield return new PuzzleInput<Puzzles.Y2020.Day24>();
        yield return new PuzzleInput<Puzzles.Y2020.Day25>();
        yield return new PuzzleInput<Puzzles.Y2021.Day01>();
        yield return new PuzzleInput<Puzzles.Y2021.Day02>();
        yield return new PuzzleInput<Puzzles.Y2021.Day03>();
        yield return new PuzzleInput<Puzzles.Y2021.Day04>();
        yield return new PuzzleInput<Puzzles.Y2021.Day05>();
        yield return new PuzzleInput<Puzzles.Y2021.Day06>();
        yield return new PuzzleInput<Puzzles.Y2021.Day07>();
        yield return new PuzzleInput<Puzzles.Y2021.Day08>();
        yield return new PuzzleInput<Puzzles.Y2021.Day09>();
        yield return new PuzzleInput<Puzzles.Y2021.Day10>();
        yield return new PuzzleInput<Puzzles.Y2021.Day11>();
        yield return new PuzzleInput<Puzzles.Y2021.Day12>();
        yield return new PuzzleInput<Puzzles.Y2021.Day13>();
        yield return new PuzzleInput<Puzzles.Y2021.Day14>();
        yield return new PuzzleInput<Puzzles.Y2021.Day15>();
        yield return new PuzzleInput<Puzzles.Y2021.Day16>();
        yield return new PuzzleInput<Puzzles.Y2021.Day17>();
        yield return new PuzzleInput<Puzzles.Y2021.Day18>();
        yield return new PuzzleInput<Puzzles.Y2021.Day19>();
        yield return new PuzzleInput<Puzzles.Y2021.Day20>();
        yield return new PuzzleInput<Puzzles.Y2021.Day21>();
        yield return new PuzzleInput<Puzzles.Y2021.Day22>();
        yield return new PuzzleInput<Puzzles.Y2021.Day24>();
        yield return new PuzzleInput<Puzzles.Y2021.Day25>();
    }

    [Benchmark]
    [ArgumentsSource(nameof(Puzzles))]
    public async Task<PuzzleResult> Solve(PuzzleInput input)
        => await input.Puzzle.SolveAsync(input.Args, CancellationToken.None);

    public sealed class PuzzleInput<T> : PuzzleInput
        where T : IPuzzle, new()
    {
        public PuzzleInput(params string[] args)
            : base(args)
        {
            Puzzle = new T() { Verbose = false };

            if (Puzzle is Puzzle puzzle)
            {
                puzzle.Logger = new NullLogger();
            }
        }

        public override IPuzzle Puzzle { get; }
    }

    public abstract class PuzzleInput
    {
        protected PuzzleInput(params string[] args)
        {
            Args = args;
        }

        public string[] Args { get; }

        public abstract IPuzzle Puzzle { get; }

        public override string ToString()
        {
            var type = Puzzle.GetType();
            string[] split = type.FullName!.Split('.');

            string year = split[3];
            string day = split[4].Replace("Day", string.Empty, StringComparison.Ordinal);

            string name = $"{year}-{day}";

            if (Args?.Length > 0)
            {
                name += "-" + string.Join("-", Args);
            }

            return name;
        }
    }

    private sealed class NullLogger : ILogger
    {
        public string WriteGrid(bool[,] array, char falseChar, char trueChar)
            => string.Empty;

        public void WriteLine(string format, params object[] args)
        {
            // No-op
        }
    }
}
