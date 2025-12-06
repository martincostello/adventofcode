// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 01, "Chronal Calibration", RequiresData = true, IsSlow = true)]
public sealed class Day01 : Puzzle<int, int>
{
    /// <summary>
    /// Calculates the resulting frequency after applying the specified sequence.
    /// </summary>
    /// <param name="sequence">A sequence of frequency shifts to apply.</param>
    /// <returns>
    /// The resulting frequency from applying the sequence from <paramref name="sequence"/>.
    /// </returns>
    public static int CalculateFrequency(IList<int> sequence)
        => CalculateFrequencyWithRepetition(sequence).Frequency;

    /// <summary>
    /// Calculates the resulting frequency after applying the specified sequence.
    /// </summary>
    /// <param name="sequence">A sequence of frequency shifts to apply.</param>
    /// <returns>
    /// The resulting frequency from applying the sequence from <paramref name="sequence"/>.
    /// </returns>
    public static (int Frequency, int FirstRepeat) CalculateFrequencyWithRepetition(IList<int> sequence)
    {
        int current = 0;
        int? frequency = null;
        int? firstRepeat = null;

        var history = new List<int>(sequence.Count)
        {
            current,
        };

        bool isInfinite = TendsToInfinity(sequence);

        do
        {
            foreach (int shift in sequence)
            {
                int previous = current;
                current += shift;

                if (!firstRepeat.HasValue && history.Contains(current) && history[^2] != previous)
                {
                    firstRepeat = current;
                }

                history.Add(current);
            }

            if (!frequency.HasValue)
            {
                frequency = current;
            }
        }
        while (!firstRepeat.HasValue && !isInfinite);

        return (frequency.Value, firstRepeat ?? frequency.Value);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var sequence = await ReadResourceAsNumbersAsync<int>(cancellationToken);

        (Solution1, Solution2) = CalculateFrequencyWithRepetition(sequence);

        if (Verbose)
        {
            Logger.WriteLine($"The resulting frequency is {Solution1:N0}.");
            Logger.WriteLine($"The first repeated frequency is {Solution2:N0}.");
        }

        return Result();
    }

    /// <summary>
    /// Returns whether the specified sequences tends towards infinity.
    /// </summary>
    /// <param name="sequence">The sequence of integers.</param>
    /// <returns>
    /// <see langword="true"/> if the sequence tends towards positive or negative infinity.
    /// </returns>
    private static bool TendsToInfinity(IList<int> sequence)
        => Math.Abs(sequence.Sum(Math.Sign)) == sequence.Count((p) => p is not 0);
}
