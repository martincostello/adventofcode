// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Point = (long X, long Y);

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 13, "Claw Contraption", RequiresData = true)]
public sealed class Day13 : Puzzle<long, long>
{
    /// <summary>
    /// Gets the fewest tokens needed to be spent to win all possible prizes.
    /// </summary>
    public long FewestTokens { get; private set; }

    /// <summary>
    /// Gets the fewest tokens needed to be spent to win all possible prizes with the correct offset.
    /// </summary>
    public long FewestTokensFixed { get; private set; }

    /// <summary>
    /// Plays the specified claw machines.
    /// </summary>
    /// <param name="machines">The specification of the claw machines to play.</param>
    /// <param name="offset">The claw offset to use.</param>
    /// <param name="limit">The limit of turns to use when finding a solution.</param>
    /// <returns>
    /// The fewest tokens needed to be spent to win all possible prizes.
    /// </returns>
    public static long Play(IList<string> machines, long offset, long limit)
    {
        var clawMachines = new List<ClawMachine>();

        for (int i = 0; i < machines.Count; i += 4)
        {
            var a = Parse(machines[i], '+');
            var b = Parse(machines[i + 1], '+');
            var prize = Parse(machines[i + 2], '=');

            prize = (prize.X + offset, prize.Y + offset);

            clawMachines.Add(new(a, b, prize));
        }

        long tokens = 0;

        foreach (var machine in clawMachines)
        {
            tokens += machine.Play(limit);
        }

        return tokens;

        static (long X, long Y) Parse(string value, char delimiter)
        {
            string coordinates = value.Split(':')[1];
            string[] parts = coordinates.Split(',');

            var rawX = parts[0].AsSpan();
            var rawY = parts[1].AsSpan();

            long x = Parse<long>(rawX[(rawX.IndexOf(delimiter) + 1)..]);
            long y = Parse<long>(rawY[(rawY.IndexOf(delimiter) + 1)..]);

            return (x, y);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        FewestTokens = Play(values, offset: 0, limit: 100);
        FewestTokensFixed = Play(values, offset: 10_000_000_000_000, limit: long.MaxValue);

        if (Verbose)
        {
            Logger.WriteLine("The fewest tokens you would have to spend to win all possible prizes is {0}.", FewestTokens);
            Logger.WriteLine("The fewest tokens you would have to spend to win all possible prizes with the correct offset is {0}.", FewestTokensFixed);
        }

        Solution1 = FewestTokens;
        Solution2 = FewestTokensFixed;

        return Result();
    }

    private sealed record ClawMachine(Point A, Point B, Point Prize)
    {
        public long Play(long limit)
        {
            // See https://en.wikipedia.org/wiki/Cramer%27s_rule#Explicit_formulas_for_small_systems
            long detAB = (A.X * B.Y) - (A.Y * B.X);
            long detAC = (A.X * Prize.Y) - (A.Y * Prize.X);

            if (detAC % detAB is not 0)
            {
                return 0;
            }

            long detCB = (Prize.X * B.Y) - (Prize.Y * B.X);

            if (detCB % detAB is not 0)
            {
                return 0;
            }

            long x = detCB / detAB;

            if (x > limit)
            {
                return 0;
            }

            long y = detAC / detAB;

            if (y > limit)
            {
                return 0;
            }

            return (x * 3) + y;
        }
    }
}
