// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

#pragma warning disable CA1852

using System.Diagnostics;
using MartinCostello.AdventOfCode;

var style = NumberStyles.Integer & ~NumberStyles.AllowLeadingSign;
var provider = CultureInfo.InvariantCulture;

if (!int.TryParse(args[0], style, provider, out int day))
{
    day = 0;
}

int year = 0;

if (args.Length > 1)
{
    if (!int.TryParse(args[1], style, provider, out year))
    {
        year = 0;
    }

    args = args[2..];
}
else
{
    year = DateTime.UtcNow.Year;
    args = args[1..];
}

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var logger = new ConsoleLogger();
var factory = new PuzzleFactory(NullCache.Instance, logger);

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

var stopwatch = Stopwatch.StartNew();

try
{
    _ = await puzzle.SolveAsync(args, cts.Token);
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

stopwatch.Stop();

logger.WriteLine();

if (stopwatch.Elapsed.TotalSeconds < 0.01f)
{
    logger.WriteLine("Took <0.01 seconds.");
}
else
{
    logger.WriteLine($"Took {stopwatch.Elapsed.TotalSeconds:N2} seconds.");
}

logger.WriteLine();

return 0;
