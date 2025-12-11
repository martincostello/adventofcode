// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/25</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 25, "Clock Signal", RequiresData = true, IsSlow = true)]
public sealed class Day25 : Puzzle<int>
{
    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            async static (instructions, logger, cancellationToken) =>
            {
                foreach (int i in Enumerable.InfiniteSequence(1, 1))
                {
                    bool isRepeating = false;
                    bool lastSignal = true;

                    int iterations = 0;

                    bool Signal(int p)
                    {
                        // If the signal is not 0 or 1, then not of interest
                        bool stop = true;

                        if ((p == 0 && lastSignal) || (p == 1 && !lastSignal))
                        {
                            // Alternation continues
                            lastSignal = !lastSignal;
                            stop = false;
                        }

                        if (!stop && iterations++ > 100)
                        {
                            // 100 alternating characters, so assume indefinite
                            isRepeating = true;
                            stop = true;
                        }

                        return stop;
                    }

                    Day12.Process(
                        instructions,
                        initialValueOfA: i,
                        signal: Signal,
                        cancellationToken: cancellationToken);

                    if (isRepeating)
                    {
                        if (logger is { })
                        {
                            logger.WriteLine($"The lowest positive integer that produces a clock signal is '{i:N0}'.");
                        }

                        return i;
                    }
                }

                return Unsolved;
            },
            cancellationToken);
    }
}
