// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 02, "Red-Nosed Reports", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// Gets the numnber of safe reports.
    /// </summary>
    public int SafeReports { get; private set; }

    /// <summary>
    /// Counts the number of reports that are safe.
    /// </summary>
    /// <param name="reports">The reports to check for safety.</param>
    /// <returns>
    /// The number of reports that are safe.
    /// </returns>
    public static int CountSafeReports(IList<string> reports)
    {
        int count = 0;

        foreach (string report in reports)
        {
            int[] values = report.Split(' ').Select(Parse<int>).ToArray();

            bool safe = true;
            int sign = Math.Sign(values[0] - values[1]);

            if (sign is 0)
            {
                continue;
            }

            for (int i = 1; i < values.Length; i++)
            {
                int left = values[i - 1];
                int right = values[i];

                int delta = Math.Abs(left - right);

                if (Math.Sign(left - right) != sign || delta > 3)
                {
                    safe = false;
                    break;
                }
            }

            if (safe)
            {
                count++;
            }
        }

        return count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SafeReports = CountSafeReports(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} reports are safe.", SafeReports);
        }

        return PuzzleResult.Create(SafeReports);
    }
}
