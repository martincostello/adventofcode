// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/12</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day12 : Puzzle2016
    {
        /// <summary>
        /// Gets the value in register A after processing the instructions.
        /// </summary>
        public int ValueInRegisterA { get; private set; }

        /// <summary>
        /// Gets the value in register A after processing the instructions
        /// if register C is initialized with the value of the position of
        /// the ignition key.
        /// </summary>
        public int ValueInRegisterAWhenInitializedWithIgnitionKey { get; private set; }

        /// <summary>
        /// Processes the specified instructions and returns the values of the CPU registers.
        /// </summary>
        /// <param name="instructions">The instructions to process.</param>
        /// <param name="initialValueOfA">The initial value of register A.</param>
        /// <param name="initialValueOfC">The initial value of register C.</param>
        /// <param name="signal">
        /// An optional delegate to receive any output clock signal; which causes signal
        /// processing to stop if it returns <see langword="true"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> containing the values of the CPU
        /// registers after processing the instructions specified by <paramref name="instructions"/>.
        /// </returns>
        internal static IDictionary<char, int> Process(
            IList<string> instructions,
            int initialValueOfA = 0,
            int initialValueOfC = 0,
            Func<int, bool> signal = null)
        {
            instructions = new List<string>(instructions); // Copy before possible modification

            IDictionary<char, int> registers = new Dictionary<char, int>()
            {
                { 'a', initialValueOfA },
                { 'b', 0 },
                { 'c', initialValueOfC },
                { 'd', 0 },
            };

            for (int i = 0; i < instructions.Count; i++)
            {
                string instruction = instructions[i];
                string[] split = instruction.Split(Arrays.Space);
                int value;

                switch (split[0])
                {
                    case "cpy":

                        if (int.TryParse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                        {
                            registers[split[2][0]] = value;
                        }
                        else
                        {
                            registers[split[2][0]] = registers[split[1][0]];
                        }

                        break;

                    case "inc":
                        registers[split[1][0]]++;
                        break;

                    case "dec":
                        registers[split[1][0]]--;
                        break;

                    case "tgl":

                        if (!TryParseInt32(split[1], out value))
                        {
                            value = registers[split[1][0]];
                        }

                        int target = i + value;

                        if (target >= 0 && target < instructions.Count)
                        {
                            string otherInstruction = instructions[target];
                            split = otherInstruction.Split(Arrays.Space);

                            string toggled;

                            if (split.Length == 2)
                            {
                                if (string.Equals(split[0], "inc", StringComparison.OrdinalIgnoreCase))
                                {
                                    toggled = $"dec {split[1]}";
                                }
                                else
                                {
                                    toggled = $"inc {split[1]}";
                                }
                            }
                            else
                            {
                                if (string.Equals(split[0], "jnz", StringComparison.OrdinalIgnoreCase))
                                {
                                    toggled = $"cpy {split[1]} {split[2]}";
                                }
                                else
                                {
                                    toggled = $"jnz {split[1]} {split[2]}";
                                }
                            }

                            instructions[target] = toggled;
                        }

                        break;

                    case "jnz":

                        if (!TryParseInt32(split[1], out value))
                        {
                            value = registers[split[1][0]];
                        }

                        int other;

                        if (!TryParseInt32(split[2], out other))
                        {
                            other = registers[split[2][0]];
                        }

                        if (value != 0)
                        {
                            i += other - 1;
                        }

                        break;

                    case "out":

                        if (!TryParseInt32(split[1], out value))
                        {
                            value = registers[split[1][0]];
                        }

                        if (signal?.Invoke(value) == true)
                        {
                            // Force the signal to stop being repeated
                            i = int.MaxValue - 1;
                            break;
                        }

                        break;

                    default:
                        break;
                }
            }

            return registers;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();

            IDictionary<char, int> registers = Process(instructions, initialValueOfC: 0);
            ValueInRegisterA = registers['a'];

            registers = Process(instructions, initialValueOfC: 1);
            ValueInRegisterAWhenInitializedWithIgnitionKey = registers['a'];

            if (Verbose)
            {
                Logger.WriteLine($"The value left in register a is {ValueInRegisterA:N0}.");
                Logger.WriteLine($"The value left in register a if c is initialized to 1 is {ValueInRegisterAWhenInitializedWithIgnitionKey:N0}.");
            }

            return 0;
        }
    }
}
