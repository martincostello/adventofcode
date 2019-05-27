// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2018/day/4</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day04 : Puzzle2018
    {
        /// <summary>
        /// Gets the product of the Id of the guard who slept the most and the minute they were asleep the most.
        /// </summary>
        public int SleepiestGuardMinute { get; private set; }

        /// <summary>
        /// Gets the product of the minute a guard was most asleep and the Id of the guard most asleep in that minute.
        /// </summary>
        public int SleepiestMinuteGuard { get; private set; }

        /// <summary>
        /// Calculates the product of the guard who slept the most and the minute they were asleep the most from the specified log.
        /// </summary>
        /// <param name="log">The log of guard activity.</param>
        /// <returns>
        /// The product of the guard who slept the most and the minute they were asleep the most as specified by <paramref name="log"/>
        /// and the product of the minute a guard was most asleep and the guard's Id.
        /// </returns>
        public static (int guardMinute, int minuteGuard) GetSleepiestGuardsMinutes(IEnumerable<string> log)
        {
            var parsedAndSortedLog = log
                .Select(LogEntry.Parse)
                .OrderBy((p) => p.Timestamp)
                .ToList();

            var groupedLog = parsedAndSortedLog.GroupBy((p) => p.Timestamp.Date);

            var guardsAsleepByMinute = groupedLog
                .Select((p) => p.Key)
                .Distinct()
                .ToDictionary((k) => k, (v) => new int[60]);

            var first = parsedAndSortedLog[0];
            int lastGuard = first.Id.Value;
            var lastTimestamp = first.Timestamp;
            bool isAwake = true;

            var midnight = new TimeSpan(0, 0, 0);
            var oneAM = new TimeSpan(1, 0, 0);
            var oneMinute = TimeSpan.FromMinutes(1);

            foreach (var day in groupedLog)
            {
                foreach (var entry in day)
                {
                    if (!isAwake &&
                        lastTimestamp.TimeOfDay >= midnight &&
                        lastTimestamp.TimeOfDay < oneAM)
                    {
                        var time = lastTimestamp;
                        int[] midnightHour = guardsAsleepByMinute[day.Key];

                        while (time.TimeOfDay < oneAM && time < entry.Timestamp)
                        {
                            midnightHour[time.TimeOfDay.Minutes] = lastGuard;
                            time = time.Add(oneMinute);
                        }
                    }

                    if (entry.Wakefulness.HasValue)
                    {
                        isAwake = entry.Wakefulness.Value;
                    }
                    else if (entry.Id.HasValue)
                    {
                        lastGuard = entry.Id.Value;
                    }

                    lastTimestamp = entry.Timestamp;
                }
            }

            var sleepinessPerGuard = parsedAndSortedLog
                .Where((p) => p.Id.HasValue)
                .Select((p) => p.Id.Value)
                .Distinct()
                .ToDictionary((k) => k, (v) => 0);

            foreach (var activity in guardsAsleepByMinute)
            {
                foreach (int guard in activity.Value.Where((p) => p != 0))
                {
                    sleepinessPerGuard[guard]++;
                }
            }

            int sleepiestGuard = sleepinessPerGuard
                .OrderByDescending((p) => p.Value)
                .Select((p) => p.Key)
                .First();

            var minutesAsleepForSleepiestGuard = new Dictionary<int, int>();
            var sleepiestGuardsByMinute = new Dictionary<int, Dictionary<int, int>>();

            foreach (var pair in guardsAsleepByMinute)
            {
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    int guard = pair.Value[i];

                    if (guard == 0)
                    {
                        continue;
                    }

                    bool isSleepiest = guard == sleepiestGuard;

                    if (isSleepiest)
                    {
                        if (!minutesAsleepForSleepiestGuard.ContainsKey(i))
                        {
                            minutesAsleepForSleepiestGuard.Add(i, 0);
                        }

                        minutesAsleepForSleepiestGuard[i]++;
                    }

                    if (!sleepiestGuardsByMinute.TryGetValue(i, out var guardsAsleepInMinute))
                    {
                        guardsAsleepInMinute = sleepiestGuardsByMinute[i] = new Dictionary<int, int>();
                    }

                    if (!guardsAsleepInMinute.ContainsKey(guard))
                    {
                        guardsAsleepInMinute.Add(guard, 0);
                    }

                    guardsAsleepInMinute[guard]++;
                }
            }

            int sleepiestMinute = minutesAsleepForSleepiestGuard
                .OrderByDescending((p) => p.Value)
                .Select((p) => p.Key)
                .First();

            int minuteGuard = sleepiestGuardsByMinute
                .OrderByDescending((p) => p.Value.Values.Max())
                .Select((p) => p.Key * p.Value.OrderByDescending((r) => r.Value).Select((r) => r.Key).First())
                .First();

            int guardMinute = sleepiestGuard * sleepiestMinute;

            return (guardMinute, minuteGuard);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> log = ReadResourceAsLines();

            (SleepiestGuardMinute, SleepiestMinuteGuard) = GetSleepiestGuardsMinutes(log);

            if (Verbose)
            {
                Logger.WriteLine($"The ID of the sleepiest guard multiplied by the most common minute is {SleepiestGuardMinute:N0}.");
                Logger.WriteLine($"The most common minute a guard was asleep in multiplied by the guard's ID is {SleepiestMinuteGuard:N0}.");
            }

            return 0;
        }

        /// <summary>
        /// A class representing a log entry. This class cannot be inherited.
        /// </summary>
        private sealed class LogEntry
        {
            /// <summary>
            /// Gets the timestamp of the log entry.
            /// </summary>
            internal DateTime Timestamp { get; private set; }

            /// <summary>
            /// Gets the Id of the guard associated with the log entry, if any.
            /// </summary>
            internal int? Id { get; private set; }

            /// <summary>
            /// Gets the wakefulness of the guard.
            /// </summary>
            internal bool? Wakefulness { get; private set; }

            /// <summary>
            /// Parses a string to an instance of <see cref="LogEntry"/>.
            /// </summary>
            /// <param name="entry">The log entry to parse.</param>
            /// <returns>
            /// The instance of <see cref="LogEntry"/> representing <paramref name="entry"/>.
            /// </returns>
            internal static LogEntry Parse(string entry)
            {
                string[] split = entry.Split(' ');

                string date1 = split[0].TrimStart('[');
                string date2 = split[1].TrimEnd(']');

                var timestamp = DateTime.ParseExact(date1 + " " + date2, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                string firstWord = split[2];

                int? id = null;
                bool? wakefulness = null;

                if (string.Equals(firstWord, "wakes", StringComparison.Ordinal))
                {
                    wakefulness = true;
                }
                else if (string.Equals(firstWord, "falls", StringComparison.Ordinal))
                {
                    wakefulness = false;
                }
                else
                {
                    id = ParseInt32(split[3].TrimStart('#'));
                }

                return new LogEntry()
                {
                    Timestamp = timestamp,
                    Id = id,
                    Wakefulness = wakefulness,
                };
            }
        }
    }
}
