// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 02, "Red-Nosed Reports", RequiresData = true)]
public sealed class Day02 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the number of safe reports.
    /// </summary>
    public int SafeReports { get; private set; }

    /// <summary>
    /// Gets the number of safe reports with the Problem Dampener in use.
    /// </summary>
    public int SafeReportsWithDampener { get; private set; }

    /// <summary>
    /// Counts the number of reports that are safe.
    /// </summary>
    /// <param name="reports">The reports to check for safety.</param>
    /// <param name="useProblemDampener">Whether to use the Problem Dampener.</param>
    /// <returns>
    /// The number of reports that are safe.
    /// </returns>
    public static int CountSafeReports(
        IList<string> reports,
        bool useProblemDampener)
    {
        int count = 0;

        foreach (string report in reports)
        {
            var values = report
                .Split(' ')
                .Select(Parse<int>)
                .ToList();

            if (IsSafe(values))
            {
                count++;
            }
            else if (useProblemDampener)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    List<int> dampened = [.. values];
                    dampened.RemoveAt(i);

                    if (IsSafe(dampened))
                    {
                        count++;
                        break;
                    }
                }
            }
        }

        return count;

        static bool IsSafe(List<int> values)
        {
            int sign = Math.Sign(values[0] - values[1]);

            for (int i = 1; i < values.Count; i++)
            {
                int left = values[i - 1];
                int right = values[i];

                int delta = Math.Abs(left - right);

                if (Math.Sign(left - right) != sign || delta > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SafeReports = CountSafeReports(values, useProblemDampener: false);
        SafeReportsWithDampener = CountSafeReports(values, useProblemDampener: true);

        if (Verbose)
        {
            Logger.WriteLine("{0} reports are safe.", SafeReports);
            Logger.WriteLine("{0} reports are safe with the Problem Dampener.", SafeReportsWithDampener);
        }

        Solution1 = SafeReports;
        Solution2 = SafeReportsWithDampener;

        return Result();
    }
}
