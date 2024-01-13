// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

namespace MartinCostello.AdventOfCode.Console;

/// <summary>
/// A class that solves a specific puzzle or all puzzles for a given year.
/// This class cannot be inherited.
/// </summary>
public sealed class PuzzleSolver
{
    private readonly int _year;
    private readonly int? _day;
    private readonly string[]? _args;
    private readonly ConsoleLogger _console = new();
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleSolver"/> class.
    /// </summary>
    /// <param name="year">The year to solve the puzzles for.</param>
    public PuzzleSolver(int year)
    {
        _year = year;
        _day = null;
        _args = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PuzzleSolver"/> class.
    /// </summary>
    /// <param name="year">The year to solve the puzzle for.</param>
    /// <param name="day">The day to solve the puzzle for.</param>
    /// <param name="args">The arguments for the puzzle.</param>
    public PuzzleSolver(int year, int day, string[] args)
    {
        _year = year;
        _day = day;
        _args = args;
    }

    private enum TimeUnit
    {
        Nanoseconds,
        Microseconds,
        Milliseconds,
        Seconds,
    }

    /// <summary>
    /// Solves the puzzle(s) associated with the solver..
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The exit code for the process.
    /// </returns>
    public async Task<int> SolveAsync(CancellationToken cancellationToken)
    {
        ILogger logger = _day.HasValue ? _console : new NullLogger();
        var factory = new PuzzleFactory(NullCache.Instance, logger);

        if (_day is { } day)
        {
            return await RunDayAsync(
                _year,
                day,
                _args ?? [],
                factory,
                logger,
                _timeProvider,
                cancellationToken);
        }
        else
        {
            return await RunYearAsyc(_year, factory, cancellationToken);
        }
    }

    private static async Task<int> RunDayAsync(
        int year,
        int day,
        string[] args,
        PuzzleFactory factory,
        ILogger logger,
        TimeProvider timeProvider,
        CancellationToken cancellationToken)
    {
        Puzzle puzzle;

        try
        {
            puzzle = factory.Create(year, day);
        }
        catch (PuzzleException ex)
        {
            logger.WriteLine(ex.Message);
            return -1;
        }

        logger.WriteLine();
        logger.WriteLine($"Advent of Code {year} - Day {day}");
        logger.WriteLine();

        long started = timeProvider.GetTimestamp();

        try
        {
            _ = await puzzle.SolveAsync(args, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger.WriteLine("Solution canceled.");
            return -1;
        }
        catch (PuzzleException ex)
        {
            logger.WriteLine(ex.Message);
            return -1;
        }

        long solved = timeProvider.GetTimestamp();
        var duration = timeProvider.GetElapsedTime(started, solved);

        logger.WriteLine();
        logger.WriteLine($"Took {Format(duration)}.");
        logger.WriteLine();

        return 0;
    }

    private static async Task<int> RunYearAsyc(int year, PuzzleFactory factory, CancellationToken cancellationToken)
    {
        var puzzles = new List<(int Day, Puzzle Puzzle)>();
        var durations = new List<TimeSpan>();

        foreach (int day in Enumerable.Range(1, 25))
        {
            try
            {
                var puzzle = factory.Create(year, day);

                if (puzzle.GetType().GetCustomAttribute<PuzzleAttribute>() is { } metadata &&
                    (metadata.IsHidden || metadata.IsSlow || metadata.Unsolved || metadata.MinimumArguments > 0))
                {
                    continue;
                }

                puzzles.Add((day, puzzle));
            }
            catch (PuzzleException)
            {
                // Not implemented yet
            }
        }

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine($"Advent of Code {year} - {puzzles.Count} Days");
        AnsiConsole.WriteLine();

        var timeProvider = TimeProvider.System;

        foreach ((_, var puzzle) in puzzles)
        {
            durations.Add(await RunPuzzleAsync(puzzle, timeProvider, cancellationToken));
        }

        var table = new Table();

        table.AddColumn("Day");
        table.AddColumn(new TableColumn("Duration").RightAligned());

        var results = puzzles.Zip(durations).Select((p) => (p.First.Day, p.First.Puzzle, p.Second));

        foreach ((int day, var puzzle, TimeSpan duration) in results)
        {
            table.AddRow(day.ToString("00", CultureInfo.InvariantCulture), Format(duration));
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        AnsiConsole.WriteLine($"Total runtime: {Format(durations.Aggregate(TimeSpan.Zero, (x, y) => x + y))}");
        AnsiConsole.WriteLine();

        return 0;
    }

    private static async Task<TimeSpan> RunPuzzleAsync(
        Puzzle puzzle,
        TimeProvider timeProvider,
        CancellationToken cancellationToken)
    {
        long started = timeProvider.GetTimestamp();

        _ = await puzzle.SolveAsync([], cancellationToken);

        long solved = timeProvider.GetTimestamp();
        return timeProvider.GetElapsedTime(started, solved);
    }

    private static TimeUnit GetUnit(TimeSpan duration)
    {
        if (duration.TotalNanoseconds < 1_000)
        {
            return TimeUnit.Nanoseconds;
        }
        else if (duration.TotalMicroseconds < 1_000)
        {
            return TimeUnit.Microseconds;
        }
        else if (duration.TotalMilliseconds < 1_000)
        {
            return TimeUnit.Milliseconds;
        }
        else
        {
            return TimeUnit.Seconds;
        }
    }

    private static string Format(TimeSpan duration)
    {
        return GetUnit(duration) switch
        {
            TimeUnit.Nanoseconds => $"{duration.Nanoseconds:N2}ns",
            TimeUnit.Microseconds => $"{duration.TotalMicroseconds:N2}μs",
            TimeUnit.Milliseconds => $"{duration.TotalMilliseconds:N2}ms",
            TimeUnit.Seconds => $"{duration.TotalSeconds:N2}s",
            _ => throw new UnreachableException(),
        };
    }

    private sealed class NullLogger : ILogger
    {
        public string WriteGrid(bool[,] array, char falseChar, char trueChar) => string.Empty;

        public void WriteLine(string format, params object[] args)
        {
            // No-op
        }
    }
}
