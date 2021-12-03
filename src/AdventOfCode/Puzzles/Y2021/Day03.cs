// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 03, RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Gets the power consumption of the submarine.
    /// </summary>
    public int PowerConsumption { get; private set; }

    /// <summary>
    /// Gets the power consumption of the submarine computed from the diagnostic report.
    /// </summary>
    /// <param name="diagnosticReport">The submarine's diagnostic report.</param>
    /// <returns>
    /// The power consumption derived from <paramref name="diagnosticReport"/>.
    /// </returns>
    public static int GetPowerConsumption(IList<string> diagnosticReport)
    {
        int gammaRate = 0;
        int epsilonRate = 0;

        int bits = diagnosticReport.FirstOrDefault()?.Length ?? 0;

        for (int i = 0; i < bits; i++)
        {
            int zeroes = 0;
            int ones = 0;

            foreach (string value in diagnosticReport)
            {
                if (value[i] == '0')
                {
                    zeroes++;
                }
                else
                {
                    ones++;
                }
            }

            bool moreOnes = ones > zeroes;
            int gamma = moreOnes ? 1 : 0;
            int epsilon = moreOnes ? 0 : 1;

            SetBit(ref gammaRate, bits - i - 1, gamma);
            SetBit(ref epsilonRate, bits - i - 1, epsilon);
        }

        return gammaRate * epsilonRate;

        static void SetBit(ref int reference, int bit, int value) => reference |= value << bit;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> diagnosticReport = await ReadResourceAsLinesAsync();

        PowerConsumption = GetPowerConsumption(diagnosticReport);

        if (Verbose)
        {
            Logger.WriteLine(
                "The power consumption of the submarine is {0:N0}.",
                PowerConsumption);
        }

        return PuzzleResult.Create(PowerConsumption);
    }
}
