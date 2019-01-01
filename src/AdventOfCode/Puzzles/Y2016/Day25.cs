﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/25</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day25 : Puzzle2016
    {
        /// <summary>
        /// Gets the minimum value that generates a clock signal.
        /// </summary>
        public int ClockSignalValue { get; private set; }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();

            for (int i = 1; i < int.MaxValue; i++)
            {
                bool isRepeating = false;

                int lastSignal = 1;
                int iterations = 0;

                bool Signal(int p)
                {
                    // If the signal is not 0 or 1, then not of interest
                    bool stop = true;

                    if (p == 0)
                    {
                        if (lastSignal == 1)
                        {
                            // Alternation continues
                            lastSignal = 0;
                            stop = false;
                        }
                    }
                    else if (p == 1)
                    {
                        if (lastSignal == 0)
                        {
                            // Alternation continues
                            lastSignal = 1;
                            stop = false;
                        }
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
                    signal: Signal);

                if (isRepeating)
                {
                    ClockSignalValue = i;
                    break;
                }
            }

            if (Verbose)
            {
                Logger.WriteLine($"The lowest positive integer that produces a clock signal is '{ClockSignalValue:N0}'.");
            }

            return 0;
        }
    }
}
