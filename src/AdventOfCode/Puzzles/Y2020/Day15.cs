// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 15, "Rambunctious Recitation", MinimumArguments = 1, IsSlow = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the 2020th number spoken.
    /// </summary>
    public int Number2020 { get; private set; }

    /// <summary>
    /// Gets the 30,000,000th number spoken.
    /// </summary>
    public int Number30000000 { get; private set; }

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
        var numbers = new Dictionary<int, List<int>>();

        int lastSpoken = 0;

        for (int i = 0; i < number && i < startingNumbers.Count; i++)
        {
            lastSpoken = startingNumbers[i];
            UpdateNumber(lastSpoken, i);
        }

        for (int i = startingNumbers.Count; i < number; i++)
        {
            var times = numbers.GetOrAdd(lastSpoken);

            if (times.Count < 2)
            {
                lastSpoken = 0;
            }
            else
            {
                lastSpoken = times[^1] - times[^2];
            }

            UpdateNumber(lastSpoken, i);
        }

        return lastSpoken;

        void UpdateNumber(int value, int time)
        {
            numbers.GetOrAdd(value).Add(time);
        }
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> startingNumbers = args[0]
            .AsNumbers<int>()
            .ToList();

        Number2020 = GetSpokenNumber(startingNumbers, 2020);
        Number30000000 = GetSpokenNumber(startingNumbers, 30000000);

        if (Verbose)
        {
            Logger.WriteLine("The 2020th number spoken is {0}.", Number2020);
            Logger.WriteLine("The 30000000th number spoken is {0}.", Number30000000);
        }

        return PuzzleResult.Create(Number2020, Number30000000);
    }
}
