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
        /// Gets the shortest delay for a trip through the firewall that has a severity of zero.
        /// </summary>
        public int ShortestDelay { get; private set; }

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
            firewall.AddPacket();

            int severity = firewall.Tick();

            while (!firewall.TripComplete)
            {
                firewall.Move();
                severity += firewall.Tick();
            }

            return severity;
        }

        /// <summary>
        /// Gets the shortest delay for a trip through the firewall with the specified scanner depth and ranges that does not capture the packet.
        /// </summary>
        /// <param name="depthRanges">A collection of scanner depths and ranges.</param>
        /// <returns>
        /// The shortest delay to a trip with the scanners specified by <paramref name="depthRanges"/> that does not capture the packet.
        /// </returns>
        public static int GetShortestDelayForNeverCaught(ICollection<string> depthRanges)
        {
            var firewall = new Firewall(depthRanges);

            for (int delay = 0; delay < int.MaxValue; delay++)
            {
                if (!IsEverCaught(firewall, delay))
                {
                    return delay;
                }

                firewall.Reset();
            }

            throw new InvalidProgramException("Failed to find delay for a trip with a severity of zero.");
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> depthRanges = ReadResourceAsLines();

            Severity = GetSeverityOfTrip(depthRanges);
            ShortestDelay = GetShortestDelayForNeverCaught(depthRanges);

            Console.WriteLine($"The severity of the trip through the firewall is {Severity:N0}.");
            Console.WriteLine($"The fewest number of picoseconds that the packet needs to be delayed by to pass through the firewall without being caught is {ShortestDelay:N0}.");

            return 0;
        }

        /// <summary>
        /// Gets whether the packet gets caught by a trip through the firewall with the specified scanner depth and ranges.
        /// </summary>
        /// <param name="firewall">The firewall to pass the packet through.</param>
        /// <param name="delay">The delay to apply before moving the packet.</param>
        /// <returns>
        /// <see langword="true"/> if the packet if ever caught; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsEverCaught(Firewall firewall, int delay)
        {
            for (int i = 0; i < delay; i++)
            {
                firewall.Tick();
            }

            firewall.AddPacket();
            firewall.Tick(out bool wasCaught);

            if (wasCaught)
            {
                return true;
            }

            while (!firewall.TripComplete)
            {
                firewall.Move();
                firewall.Tick(out wasCaught);

                if (wasCaught)
                {
                    return true;
                }
            }

            return false;
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
            /// Gets or sets the number of ticks that have elapsed.
            /// </summary>
            internal int Ticks { get; set; }

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
            private int? Index { get; set; }

            /// <summary>
            /// Adds the packet to the firewall.
            /// </summary>
            internal void AddPacket() => Index = 0;

            /// <summary>
            /// Moves the packet forward.
            /// </summary>
            internal void Move() => Index++;

            /// <summary>
            /// Resets the firewall.
            /// </summary>
            internal void Reset()
            {
                Index = null;
                Ticks = 0;

                foreach (var scanner in Scanners.Values)
                {
                    scanner?.Reset();
                }
            }

            /// <summary>
            /// Increments the clock associated with the firewall.
            /// </summary>
            /// <returns>
            /// The severity of the current packet's trip through the firewall.
            /// </returns>
            internal int Tick() => Tick(out var _);

            /// <summary>
            /// Increments the clock associated with the firewall.
            /// </summary>
            /// <param name="wasCaught">Whether the packet was caught by the firewall.</param>
            /// <returns>
            /// The severity of the current packet's trip through the firewall.
            /// </returns>
            internal int Tick(out bool wasCaught)
            {
                wasCaught = false;

                Ticks++;

                int severity = 0;

                if (Index.HasValue &&
                    Scanners.TryGetValue(Index.Value, out var scanner) &&
                    scanner?.IsAtTop == true)
                {
                    severity = scanner.Severity;
                    wasCaught = true;
                }

                foreach (var pair in Scanners.Values)
                {
                    pair?.Tick();
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
            /// Resets the scanner.
            /// </summary>
            internal void Reset()
            {
                Incrementing = false;
                Index = 0;
                Ticks = 0;
            }

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
