// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle("Sonar Sweep", 2021, 01, RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the number of times the recorded depth increases.
    /// </summary>
    public int DepthIncreases { get; private set; }

    /// <summary>
    /// Gets the number of times the recorded depth increases over a sliding window.
    /// </summary>
    public int DepthIncreasesWithSlidingWindow { get; private set; }

    /// <summary>
    /// Gets the number of times the depth increases in the specified sequence of depth measurements.
    /// </summary>
    /// <param name="depthMeasurements">The depth measurements to count the increases in.</param>
    /// <param name="useSlidingWindow">Whether to use a sliding window of 3 measurements to measure the increase.</param>
    /// <returns>
    /// The number of times the depth increases in the sequence specified by <paramref name="depthMeasurements"/>.
    /// </returns>
    public static int GetDepthMeasurementIncreases(IList<int> depthMeasurements, bool useSlidingWindow)
    {
        if (useSlidingWindow)
        {
            var aggregated = new List<int>(depthMeasurements.Count - 2);

            for (int i = 0; i < depthMeasurements.Count - 2; i++)
            {
                aggregated.Add(
                    depthMeasurements[i] +
                    depthMeasurements[i + 1] +
                    depthMeasurements[i + 2]);
            }

            depthMeasurements = aggregated;
        }

        int result = 0;

        for (int i = 1; i < depthMeasurements.Count; i++)
        {
            int last = depthMeasurements[i - 1];
            int current = depthMeasurements[i];

            if (current > last)
            {
                result++;
            }
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> values = await ReadResourceAsNumbersAsync<int>();

        DepthIncreases = GetDepthMeasurementIncreases(values, useSlidingWindow: false);
        DepthIncreasesWithSlidingWindow = GetDepthMeasurementIncreases(values, useSlidingWindow: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The depth measurement increases {0:N0} times.",
                DepthIncreases);

            Logger.WriteLine(
                "The depth measurement increases {0:N0} times when using a sliding window of 3 measurements.",
                DepthIncreasesWithSlidingWindow);
        }

        return PuzzleResult.Create(DepthIncreases, DepthIncreasesWithSlidingWindow);
    }
}
