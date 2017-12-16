// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/13</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day13 : Puzzle2017
    {
        /// <summary>
        /// Gets the severity of the trip through the firewall.
        /// </summary>
        public int Severity { get; private set; }

        /// <summary>
        /// Gets the severity of a trip through the firewall with the specified scanner depth and ranges.
        /// </summary>
        /// <param name="depthRanges">A collection of scanner depths and ranges.</param>
        /// <returns>
        /// The severity of the trip through the firewall with the scanners specified by <paramref name="depthRanges"/>.
        /// </returns>
        public static int GetSeverityOfTrip(ICollection<string> depthRanges)
        {
            var firewall = new Firewall(depthRanges);

            int severity = 0;

            while (!firewall.TripComplete)
            {
                severity += firewall.Tick();
            }

            return severity;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> depthRanges = ReadResourceAsLines();

            Severity = GetSeverityOfTrip(depthRanges);

            Console.WriteLine($"The severity of the trip through the firewall is {Severity:N0}.");

            return 0;
        }

        /// <summary>
        /// A class representing the firewall. This class cannot be inherited.
        /// </summary>
        private sealed class Firewall
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Firewall"/> class.
            /// </summary>
            /// <param name="depthRanges">A collection of scanner depths and ranges.</param>
            internal Firewall(ICollection<string> depthRanges)
            {
                var configuration = depthRanges
                    .Select((p) => p.Split(Arrays.Colon))
                    .Select((p) => Tuple.Create(ParseInt32(p[0]), ParseInt32(p[1].Trim())))
                    .ToDictionary((p) => p.Item1, (p) => p.Item2);

                int length = configuration.Max((p) => p.Key) + 1;

                for (int depth = 0; depth < length; depth++)
                {
                    Scanner scanner = null;

                    if (configuration.TryGetValue(depth, out int range))
                    {
                        scanner = new Scanner(depth, range);
                    }

                    Scanners.Add(depth, scanner);
                }
            }

            /// <summary>
            /// Gets a value indicating whether the packet's trip through the firewall is complete.
            /// </summary>
            internal bool TripComplete => Index >= Scanners.Count - 1;

            /// <summary>
            /// Gets the scanners associated with the firewall.
            /// </summary>
            private IDictionary<int, Scanner> Scanners { get; } = new Dictionary<int, Scanner>();

            /// <summary>
            /// Gets or sets the current index of the packet travelling through the firewall.
            /// </summary>
            private int Index { get; set; }

            /// <summary>
            /// Gets or sets the number of ticks that have elapsed.
            /// </summary>
            private int Ticks { get; set; }

            /// <summary>
            /// Increments the clock associated with the firewall.
            /// </summary>
            /// <returns>
            /// The severity of the current packet's trip through the firewall.
            /// </returns>
            internal int Tick()
            {
                Index++;
                Ticks++;

                foreach (var pair in Scanners.Values)
                {
                    pair?.Tick();
                }

                int severity = 0;

                if (Scanners.TryGetValue(Index, out var scanner) && scanner?.IsAtTop == true)
                {
                    severity = scanner.Severity;
                }

                return severity;
            }
        }

        /// <summary>
        /// A class representing a scanner. This class cannot be inherited.
        /// </summary>
        private sealed class Scanner
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Scanner"/> class.
            /// </summary>
            /// <param name="depth">The depth of the scanner.</param>
            /// <param name="range">The range of the scanner.</param>
            internal Scanner(int depth, int range)
            {
                Range = range;
                Severity = depth * range;
            }

            /// <summary>
            /// Gets a value indicating whether the scanner is at the top of its range.
            /// </summary>
            internal bool IsAtTop => Index == 0;

            /// <summary>
            /// Gets the severity of the scanner.
            /// </summary>
            internal int Severity { get; }

            /// <summary>
            /// Gets or sets the current index of the scanner.
            /// </summary>
            private int Index { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the index is currently incrementing.
            /// </summary>
            private bool Incrementing { get; set; }

            /// <summary>
            /// Gets the range of the scanner.
            /// </summary>
            private int Range { get; }

            /// <summary>
            /// Gets or sets the number of ticks that have elapsed.
            /// </summary>
            private int Ticks { get; set; }

            /// <summary>
            /// Increments the clock associated with the scanner.
            /// </summary>
            internal void Tick()
            {
                Ticks++;

                if (Index == 0 || Index == Range - 1)
                {
                    Incrementing = !Incrementing;
                }

                if (Incrementing)
                {
                    Index++;
                }
                else
                {
                    Index--;
                }
            }
        }
    }
}
