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
        /// <param name="initialValueOfC">The initial value of register C.</param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> containing the values of the CPU
        /// registers after processing the instructions specified by <paramref name="instructions"/>.
        /// </returns>
        internal static IDictionary<char, int> Process(IList<string> instructions, int initialValueOfC)
        {
            IDictionary<char, int> registers = new Dictionary<char, int>()
            {
                { 'a', 0 },
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

                    case "jnz":

                        if (!int.TryParse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                        {
                            value = registers[split[1][0]];
                        }

                        if (value != 0)
                        {
                            i += ParseInt32(split[2]) - 1;
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

            Console.WriteLine($"The value left in register a is {ValueInRegisterA:N0}.");
            Console.WriteLine($"The value left in register a if c is initialized to 1 is {ValueInRegisterAWhenInitializedWithIgnitionKey:N0}.");

            return 0;
        }
    }
}
