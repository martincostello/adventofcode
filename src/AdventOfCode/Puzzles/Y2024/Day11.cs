// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 11, "Plutonian Pebbles", RequiresData = true)]
public sealed class Day11 : Puzzle<long, long>
{
    /// <summary>
    /// Counts the number of stones after the specified number of blinks.
    /// </summary>
    /// <param name="sequence">The current arrangement of the stones.</param>
    /// <param name="blinks">The number of times to blink.</param>
    /// <returns>
    /// The number of stones in the arrangement after <paramref name="blinks"/> blinks.
    /// </returns>
    public static long Blink(string sequence, int blinks)
    {
        List<long> stones = sequence.AsNumbers<long>(' ');

        Dictionary<long, long> cache = [];

        long sum = 0;

        for (int i = 0; i < stones.Count; i++)
        {
            sum += Count(stones[i], blinks, cache);
        }

        return sum;

        static long Count(long value, int blinks, Dictionary<long, long> cache)
        {
            long key = (value * 1000) + blinks;

            if (cache.TryGetValue(key, out long count))
            {
                return count;
            }

            int digits = (int)Math.Log10(value) + 1;

            if (blinks is 1)
            {
                count = digits % 2 == 0 ? 2 : 1;
            }
            else
            {
                int next = blinks - 1;

                if (value is 0)
                {
                    count = Count(1, next, cache);
                }
                else if (digits % 2 is 0)
                {
                    int midpoint = digits / 2;
                    int divisor = (int)Math.Pow(10, midpoint);

                    count = Count(value / divisor, next, cache);
                    count += Count(value % divisor, next, cache);
                }
                else
                {
                    count = Count(value * 2024, next, cache);
                }
            }

            cache[key] = count;

            return count;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string stones = await ReadResourceAsStringAsync(cancellationToken);

        Solution1 = Blink(stones, blinks: 25);
        Solution2 = Blink(stones, blinks: 75);

        if (Verbose)
        {
            Logger.WriteLine("There are {0} stones after blinking 25 times", Solution1);
            Logger.WriteLine("There are {0} stones after blinking 75 times", Solution2);
        }

        return Result();
    }
}
