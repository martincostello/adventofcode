// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 15, "Rambunctious Recitation", MinimumArguments = 1)]
public sealed class Day15 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the value of the specified number to be spoken.
    /// </summary>
    /// <param name="startingNumbers">The starting numbers for the game.</param>
    /// <param name="number">The number to get the value for.</param>
    /// <returns>
    /// The number spoken on the turn specified by <paramref name="number"/>.
    /// </returns>
    public static int GetSpokenNumber(IList<int> startingNumbers, int number)
    {
        // A number spoken after the starting numbers is the difference between
        // two turns, so it can never be larger than the number of turns. This
        // means an array indexed by the number is sufficient to track the turn
        // (one-based, so zero means never spoken) a number was last spoken on.
        int maximum = number;

        for (int i = 0; i < startingNumbers.Count; i++)
        {
            maximum = Math.Max(maximum, startingNumbers[i]);
        }

        int[] lastSpokenOn = new int[maximum + 1];

        int lastSpoken = 0;
        int turn = 1;

        for (; turn <= number && turn <= startingNumbers.Count; turn++)
        {
            if (turn > 1)
            {
                lastSpokenOn[lastSpoken] = turn - 1;
            }

            lastSpoken = startingNumbers[turn - 1];
        }

        for (; turn <= number; turn++)
        {
            int previous = turn - 1;
            int spokenOn = lastSpokenOn[lastSpoken];

            lastSpokenOn[lastSpoken] = previous;

            lastSpoken = spokenOn == 0 ? 0 : previous - spokenOn;
        }

        return lastSpoken;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken) => SolveWithArgument(args, static (argument, logger) =>
    {
        var startingNumbers = argument.AsNumbers<int>();

        int number2020 = GetSpokenNumber(startingNumbers, 2020);
        int number30000000 = GetSpokenNumber(startingNumbers, 30000000);

        if (logger is { })
        {
            logger.WriteLine("The 2020th number spoken is {0}.", number2020);
            logger.WriteLine("The 30000000th number spoken is {0}.", number30000000);
        }

        return (number2020, number30000000);
    });
}
