// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 04, "Secure Container", MinimumArguments = 1)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the number of valid passwords in the given range for version 1 of the rules.
    /// </summary>
    public int CountV1 { get; private set; }

    /// <summary>
    /// Gets the number of valid passwords in the given range for version 2 of the rules.
    /// </summary>
    public int CountV2 { get; private set; }

    /// <summary>
    /// Gets the number of valid passwords in the specified range.
    /// </summary>
    /// <param name="range">The range of numbers to get the number of passwords for.</param>
    /// <param name="rulesVersion">The version of the rules to use.</param>
    /// <returns>
    /// The Manhattan distance from the central port to the closest intersection and the
    /// minimum number of combined steps to reach an intersection.
    /// </returns>
    public static int GetPasswordsInRange(string range, int rulesVersion)
    {
        (string first, string second) = range.Bifurcate('-');

        int start = Parse<int>(first);
        int end = Parse<int>(second) + 1;

        int count = 0;

        for (int i = start; i < end; i++)
        {
            if (IsValid(i.ToString(CultureInfo.InvariantCulture), rulesVersion))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Determines whether the specified password is valid.
    /// </summary>
    /// <param name="password">The password to test for validity.</param>
    /// <param name="rulesVersion">The version of the rules to use.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="password"/> is valid; otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsValid(string password, int rulesVersion)
    {
        bool foundAdjacent = false;

        for (int i = 0; i < password.Length - 1; i++)
        {
            int first = password[i] - '0';
            int second = password[i + 1] - '0';

            if (first == second)
            {
                foundAdjacent = true;
            }

            if (first > second)
            {
                return false;
            }
        }

        if (foundAdjacent && rulesVersion > 1)
        {
            char last = password[0];
            int run = 1;
            var runs = new List<int>();

            for (int i = 1; i < password.Length; i++)
            {
                char current = password[i];

                if (password[i] == last)
                {
                    run++;
                }
                else
                {
                    runs.Add(run);
                    run = 1;
                    last = current;
                }
            }

            runs.Add(run);

            if (runs.Any((p) => p > 2) && !runs.Any((p) => p == 2))
            {
                return false;
            }
        }

        return foundAdjacent;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        CountV1 = GetPasswordsInRange(args[0], rulesVersion: 1);
        CountV2 = GetPasswordsInRange(args[0], rulesVersion: 2);

        if (Verbose)
        {
            Logger.WriteLine("{0} different passwords within the range meet the criteria for version 1.", CountV1);
            Logger.WriteLine("{0} different passwords within the range meet the criteria for version 2.", CountV2);
        }

        return PuzzleResult.Create(CountV1, CountV2);
    }
}
