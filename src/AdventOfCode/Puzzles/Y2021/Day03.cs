// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 03, RequiresData = true)]
public sealed class Day03 : Puzzle
{
    private const char Zero = '0';
    private const char One = '1';

    /// <summary>
    /// Gets the power consumption of the submarine.
    /// </summary>
    public int PowerConsumption { get; private set; }

    /// <summary>
    /// Gets the life support rating of the submarine.
    /// </summary>
    public int LifeSupportRating { get; private set; }

    /// <summary>
    /// Gets the power consumption of the submarine computed from the diagnostic report.
    /// </summary>
    /// <param name="diagnosticReport">The submarine's diagnostic report.</param>
    /// <returns>
    /// The power consumption derived from <paramref name="diagnosticReport"/>.
    /// </returns>
    public static int GetPowerConsumption(IList<string> diagnosticReport)
    {
        int bits = diagnosticReport.FirstOrDefault()?.Length ?? 0;

        int gammaRate = 0;
        int epsilonRate = 0;

        for (int bit = 0; bit < bits; bit++)
        {
            (int zeroes, int ones) = CountBits(diagnosticReport, bit);

            bool moreOnes = ones > zeroes;
            int gamma = moreOnes ? 1 : 0;
            int epsilon = moreOnes ? 0 : 1;

            SetBit(ref gammaRate, bits - bit - 1, gamma);
            SetBit(ref epsilonRate, bits - bit - 1, epsilon);
        }

        return gammaRate * epsilonRate;
    }

    /// <summary>
    /// Gets the life support rating of the submarine computed from the diagnostic report.
    /// </summary>
    /// <param name="diagnosticReport">The submarine's diagnostic report.</param>
    /// <returns>
    /// The life support rating derived from <paramref name="diagnosticReport"/>.
    /// </returns>
    public static int GetLifeSupportRating(IList<string> diagnosticReport)
    {
        int bits = diagnosticReport.FirstOrDefault()?.Length ?? 0;

        int oxygenRating = GetRating(bits, diagnosticReport, One, Zero);
        int co2ScrubberRating = GetRating(bits, diagnosticReport, Zero, One);

        return oxygenRating * co2ScrubberRating;

        static int GetRating(int bits, IList<string> values, char on, char off)
        {
            var candidates = new List<string>(values);

            for (int bit = 0; bit < bits; bit++)
            {
                (int zeroes, int ones) = CountBits(candidates, bit);

                char ratingBit = ones >= zeroes ? on : off;

                if (candidates.Count > 1)
                {
                    candidates.RemoveAll((p) => p[bit] != ratingBit);
                }
            }

            string rating = candidates.Single();

            int result = 0;

            for (int i = 0; i < rating.Length; i++)
            {
                SetBit(ref result, bits - i - 1, rating[i] == Zero ? 0 : 1);
            }

            return result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> diagnosticReport = await ReadResourceAsLinesAsync();

        PowerConsumption = GetPowerConsumption(diagnosticReport);
        LifeSupportRating = GetLifeSupportRating(diagnosticReport);

        if (Verbose)
        {
            Logger.WriteLine(
                "The power consumption of the submarine is {0:N0}.",
                PowerConsumption);

            Logger.WriteLine(
                "The life support rating of the submarine is {0:N0}.",
                LifeSupportRating);
        }

        return PuzzleResult.Create(PowerConsumption, LifeSupportRating);
    }

    private static (int Zeroes, int Ones) CountBits(IEnumerable<string> values, int bit)
    {
        int zeroes = 0;
        int ones = 0;

        foreach (string value in values)
        {
            if (value[bit] == Zero)
            {
                zeroes++;
            }
            else
            {
                ones++;
            }
        }

        return (zeroes, ones);
    }

    private static void SetBit(ref int reference, int bit, int value)
        => reference |= value << bit;
}
