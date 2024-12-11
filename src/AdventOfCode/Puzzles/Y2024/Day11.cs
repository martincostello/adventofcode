// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 11, "Plutonian Pebbles", RequiresData = true)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// Gets the number of stones after 25 blinks.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Counts the number of stones after the specified number of blinks.
    /// </summary>
    /// <param name="sequence">The current arrangement of the stones.</param>
    /// <param name="blinks">The number of times to blink.</param>
    /// <returns>
    /// The number of stones in the arrangement after <paramref name="blinks"/> blinks.
    /// </returns>
    public static int Blink(string sequence, int blinks)
    {
        List<long> stones = sequence.AsNumbers<long>(' ');

        for (int i = 0; i < blinks; i++)
        {
            for (int j = 0; j < stones.Count; j++)
            {
                long stone = stones[j];

                if (stone is 0)
                {
                    stones[j] = 1;
                }
                {
                    var digits = Maths.Digits(stone);

                    stones[j] = Maths.FromDigits<long>(digits[0..(digits.Count / 2)]);
                    stones.Insert(++j, Maths.FromDigits<long>(digits[(digits.Count / 2)..]));
                }
                else
                {
                    stones[j] *= 2024;
                }
            }
        }

        return stones.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string stones = await ReadResourceAsStringAsync(cancellationToken);

        Count = Blink(stones, blinks: 25);

        if (Verbose)
        {
            Logger.WriteLine("There are {0} stones after blinking 25 times", Count);
        }

        return PuzzleResult.Create(Count);
    }
}
