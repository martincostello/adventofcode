// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/11</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 11, RequiresData = true, IsHidden = true)]
    public sealed class Day11 : Puzzle
    {
        /// <summary>
        /// Gets the minimum number of steps required to assemble all the input generators.
        /// </summary>
        public int MinimumStepsForAssembly { get; private set; }

        /// <summary>
        /// Gets the minimum number of steps required to assemble all the found generators.
        /// </summary>
        public int MinimumStepsForAssemblyWithExtraParts { get; private set; }

        /// <summary>
        /// Returns the minimum number of steps required to assemble all the generators
        /// on the fourth floor of the building given the specified initial state.
        /// </summary>
        /// <param name="initialState">The initial state of the generators and microchips.</param>
        /// <param name="useExtra">Whether to use the extra parts found on the first floor.</param>
        /// <returns>
        /// The minimum number of steps required to assemble all the generators given <paramref name="initialState"/>.
        /// </returns>
        internal static int GetMinimumStepsForAssembly(IList<string> initialState, bool useExtra)
        {
            (BitArray state, int elements) = ParseInitialState(initialState);

            if (useExtra)
            {
                state.Set(16 + elements - 1, true);     // Dilithium generator
                state.Set(16 + elements, true);         // Elerium generator
                state.Set(16 + 8 + elements - 1, true); // Elerium-compatible microchip
                state.Set(16 + 8 + elements, true);     // Dilithium-compatible microchip
                elements += 2;
            }

            long state64 = ToInt64(state);

            var frontier = new Queue<long>();
            frontier.Enqueue(state64);

            var reached = new HashSet<long>() { state64 };

            int floor = 0;

            while (frontier.Count > 0)
            {
                BitArray current = FromInt64(frontier.Dequeue());

                // Up and down
                for (int i = 0; i < 2; i++)
                {
                    bool up = i == 0;

                    if (up && floor == 0)
                    {
                        continue;
                    }
                    else if (floor == 3)
                    {
                        continue;
                    }

                    for (int generator = 0; generator < elements; generator++)
                    {
                        for (int microchip = 0; microchip < elements; microchip++)
                        {
                            bool isValidMove = false;

                            if (isValidMove)
                            {
                                var newState = new BitArray(current);

                                newState.Set(generator, up);
                                newState.Set(generator - 16, !up);

                                newState.Set(microchip, up);
                                newState.Set(microchip - 16, !up);

                                frontier.Enqueue(ToInt64(newState));
                            }
                        }
                    }
                }

                ////foreach (long next in graph.Neighbors(current))
                ////{
                ////    if (!reached.Contains(next))
                ////    {
                ////        frontier.Enqueue(next);
                ////        reached.Add(next);
                ////    }
                ////}
            }

            return 0;

            static BitArray FromInt64(long value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                return new BitArray(bytes);
            }

            static long ToInt64(BitArray bits)
            {
                byte[] array = new byte[8];

                bits.CopyTo(array, 0);

                return BitConverter.ToInt64(array);
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> initialState = await ReadResourceAsLinesAsync();

            MinimumStepsForAssembly = GetMinimumStepsForAssembly(initialState, useExtra: false);
            MinimumStepsForAssemblyWithExtraParts = GetMinimumStepsForAssembly(initialState, useExtra: true);

            if (Verbose)
            {
                Logger.WriteLine(
                    $"The minimum number of steps required to bring all of the input objects to the fourth floor is {0:N0}.",
                    MinimumStepsForAssembly);

                Logger.WriteLine(
                    $"The minimum number of steps required to bring all of the found objects to the fourth floor is {0:N0}.",
                    MinimumStepsForAssemblyWithExtraParts);
            }

            return PuzzleResult.Create(MinimumStepsForAssembly, MinimumStepsForAssemblyWithExtraParts);
        }

        /// <summary>
        /// Parses the initial state of the Radioisotope Testing Facility.
        /// </summary>
        /// <param name="initialState">The initial state of the facility.</param>
        /// <returns>
        /// A <see cref="BitArray"/> value representing the state of the facility
        /// which encodes the floors on which the generators and microchips are located
        /// and the number of unique elements present in the facility.
        /// </returns>
        private static (BitArray state, int elements) ParseInitialState(IList<string> initialState)
        {
            int floors = initialState.Count;

            var elements = new List<string>();
            var generators = new Dictionary<int, IList<string>>();
            var microchips = new Dictionary<int, IList<string>>();

            for (int floor = 0; floor < floors; floor++)
            {
                generators[floor] = new List<string>();
                microchips[floor] = new List<string>();

                string[] split = initialState[floor].TrimEnd('.').Split(' ');

                for (int j = 0; j < split.Length; j++)
                {
                    string value = split[j].TrimEnd(',');
                    string? element = null;

                    if (string.Equals(value, "generator", StringComparison.Ordinal))
                    {
                        element = split[j - 1];
                        generators[floor].Add(element);
                    }
                    else if (string.Equals(value, "microchip", StringComparison.Ordinal))
                    {
                        element = split[j - 1].Replace("-compatible", string.Empty, StringComparison.Ordinal);
                        microchips[floor].Add(element);
                    }

                    if (element is not null && !elements.Contains(element))
                    {
                        elements.Add(element);
                    }
                }
            }

            var state = new BitArray(64);

            for (int i = 0; i < elements.Count; i++)
            {
                string element = elements[i];

                for (int floor = 0; floor < floors; floor++)
                {
                    int offset = floor * 16;
                    int bit = offset + i;

                    if (generators[floor].Contains(element))
                    {
                        state.Set(bit, true);
                    }

                    if (microchips[floor].Contains(element))
                    {
                        state.Set(bit + 8, true);
                    }
                }
            }

            return (state, elements.Count);
        }
    }
}
