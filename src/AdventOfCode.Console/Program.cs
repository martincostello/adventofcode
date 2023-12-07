// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using MartinCostello.AdventOfCode.Console;

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
else if (day >= 2015)
{
    year = day;
    day = 0;
    args = [];
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

PuzzleSolver solver;

if (day == 0)
{
    solver = new(year);
}
else
{
    solver = new(year, day, args);
}

return await solver.SolveAsync(cts.Token);
