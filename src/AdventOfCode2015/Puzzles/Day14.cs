// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/14</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day14 : IPuzzle
    {
        /// <summary>
        /// Gets the maximum distance travelled by the winning reindeed at the given point in time, in kilometers.
        /// </summary>
        internal int MaximumReindeerDistance { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No input file path or time index specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            int timeIndex;

            if (!int.TryParse(args[1], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out timeIndex) ||
                timeIndex < 0)
            {
                Console.Error.WriteLine("The time index specified is invalid.");
                return -1;
            }

            string[] flightData = File.ReadAllLines(args[0]);

            MaximumReindeerDistance = GetMaximumDistanceOfFastestReindeer(flightData, timeIndex);

            Console.WriteLine("After {0:N0} seconds, the furthest reindeer is {1:N0} km away.", timeIndex, MaximumReindeerDistance);

            return 0;
        }

        /// <summary>
        /// Gets the maximum distance travelled by the specified reindeed at the specified time index.
        /// </summary>
        /// <param name="flightData">The reindeed flight data.</param>
        /// <param name="timeIndex">The time index.</param>
        /// <returns>
        /// The maximum distance travelled by a reindeed at the time index specified by <paramref name="timeIndex"/>.
        /// </returns>
        internal static int GetMaximumDistanceOfFastestReindeer(ICollection<string> flightData, int timeIndex)
        {
            List<FlightData> data = flightData
                .Select(FlightData.Parse)
                .ToList();

            return data
                .Select((p) => p.GetDistanceAfterTimeIndex(timeIndex))
                .Max();
        }

        /// <summary>
        /// A class representing flight data for a reindeer. This class cannot be inherited.
        /// </summary>
        internal sealed class FlightData
        {
            /// <summary>
            /// Gets or sets the name of the reindeer.
            /// </summary>
            internal string Name { get; set; }

            /// <summary>
            /// Gets or sets the maximum speed of the reindeer.
            /// </summary>
            internal int MaximumSpeed { get; set; }

            /// <summary>
            /// Gets or sets the maximum activity period of the reindeed, in seconds.
            /// </summary>
            internal int MaximumActivityPeriod { get; set; }

            /// <summary>
            /// Gets or sets the rest period of the reindeed, in seconds.
            /// </summary>
            internal int RestPeriod { get; set; }

            /// <summary>
            /// Parses an instance of <see cref="FlightData"/> from a <see cref="string"/>.
            /// </summary>
            /// <param name="value">The value to parse.</param>
            /// <returns>
            /// A <see cref="FlightData"/> instance representing the value parsed from <paramref name="value"/>.
            /// </returns>
            internal static FlightData Parse(string value)
            {
                string[] split = value.Split(' ');

                return new FlightData()
                {
                    Name = split.First(),
                    MaximumSpeed = int.Parse(split[3], CultureInfo.InvariantCulture),
                    MaximumActivityPeriod = int.Parse(split[6], CultureInfo.InvariantCulture),
                    RestPeriod = int.Parse(split[13], CultureInfo.InvariantCulture),
                };
            }

            /// <summary>
            /// Gets the distance the reindeed has travelled at the specified time index.
            /// </summary>
            /// <param name="timeIndex">The time index to get the distance for.</param>
            /// <returns>
            /// The distance travelled by the reindeed at the time index specified by <paramref name="timeIndex"/>.
            /// </returns>
            internal int GetDistanceAfterTimeIndex(int timeIndex)
            {
                int cycleTime = MaximumActivityPeriod + RestPeriod;

                int cycles = timeIndex / cycleTime;
                int secondsOfIncompleteCycle = timeIndex % cycleTime;

                int secondsOfTravelInIncompleteCycle =
                    secondsOfIncompleteCycle > MaximumActivityPeriod ?
                    MaximumActivityPeriod :
                    secondsOfIncompleteCycle;

                return (cycles * MaximumActivityPeriod * MaximumSpeed) + (secondsOfTravelInIncompleteCycle * MaximumSpeed);
            }
        }
    }
}
