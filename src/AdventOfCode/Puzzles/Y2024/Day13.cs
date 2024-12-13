// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 13, "Claw Contraption", RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the fewest tokens needed to be spent to win all possible prizes.
    /// </summary>
    public int FewestTokens { get; private set; }

    /// <summary>
    /// Plays the specified claw machines.
    /// </summary>
    /// <param name="machines">The specification of the claw machines to play.</param>
    /// <returns>
    /// The fewest tokens needed to be spent to win all possible prizes.
    /// </returns>
    public static int Play(IList<string> machines)
    {
        var clawMachines = new List<ClawMachine>();

        for (int i = 0; i < machines.Count; i += 4)
        {
            var a = Parse(machines[i], '+');
            var b = Parse(machines[i + 1], '+');
            var prize = Parse(machines[i + 2], '=');

            clawMachines.Add(new(a, b, prize));
        }

        int tokens = 0;

        foreach (var machine in clawMachines)
        {
            tokens += machine.Play();
        }

        return tokens;

        static Point Parse(string value, char delimiter)
        {
            string coordinates = value.Split(':')[1];
            string[] parts = coordinates.Split(',');

            var rawX = parts[0].AsSpan();
            var rawY = parts[1].AsSpan();

            int x = Parse<int>(rawX[(rawX.IndexOf(delimiter) + 1)..]);
            int y = Parse<int>(rawY[(rawY.IndexOf(delimiter) + 1)..]);

            return new(x, y);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        FewestTokens = Play(values);

        if (Verbose)
        {
            Logger.WriteLine("The fewest tokens you would have to spend to win all possible prizes is {0}.", FewestTokens);
        }

        return PuzzleResult.Create(FewestTokens);
    }

    private sealed record ClawMachine(Point A, Point B, Point Prize)
    {
        public int Play()
        {
            // See https://en.wikipedia.org/wiki/Cramer%27s_rule#Explicit_formulas_for_small_systems
            int detAB = (A.X * B.Y) - (A.Y * B.X);
            int detAC = (A.X * Prize.Y) - (A.Y * Prize.X);

            if (detAC % detAB is not 0)
            {
                return 0;
            }

            int detCB = (Prize.X * B.Y) - (Prize.Y * B.X);

            if (detCB % detAB is not 0)
            {
                return 0;
            }

            int x = detCB / detAB;

            if (x > 100)
            {
                return 0;
            }

            int y = detAC / detAB;

            if (y > 100)
            {
                return 0;
            }

            return (x * 3) + y;
        }
    }
}
