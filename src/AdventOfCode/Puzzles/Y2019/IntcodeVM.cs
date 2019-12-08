// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A class representing an Intcode Virtual Machine. This class cannot be inherited.
    /// </summary>
    internal static class IntcodeVM
    {
        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <param name="output">The output from the program.</param>
        /// <returns>
        /// The memory values of the program once run.
        /// </returns>
        internal static IReadOnlyList<int> Run(ReadOnlySpan<int> program, IEnumerable<int> input, out int output)
        {
            output = 0;

            int[] memory = program.ToArray();
            using var enumerator = input.GetEnumerator();

            void Add(int current, int[] modes)
            {
                int x = memory[current + 1];
                int y = memory[current + 2];
                int z = memory[current + 3];

                int left = modes[0] == 0 ? memory[x] : x;
                int right = modes[1] == 0 ? memory[y] : y;

                memory[z] = left + right;
            }

            void Multiply(int counter, int[] modes)
            {
                int x = memory[counter + 1];
                int y = memory[counter + 2];
                int z = memory[counter + 3];

                int left = modes[0] == 0 ? memory[x] : x;
                int right = modes[1] == 0 ? memory[y] : y;

                memory[z] = left * right;
            }

            void Input(int current, int value)
            {
                int x = memory[current + 1];
                memory[x] = value;
            }

            int Output(int current, int[] modes)
            {
                int x = memory[current + 1];
                return modes[0] == 0 ? memory[x] : x;
            }

            void JumpIfTrue(ref int current, int[] modes)
            {
                int x = memory[current + 1];

                if ((modes[0] == 0 ? memory[x] : x) != 0)
                {
                    int y = memory[current + 2];
                    current = modes[1] == 0 ? memory[y] : y;
                }
                else
                {
                    current += 3;
                }
            }

            void JumpIfFalse(ref int current, int[] modes)
            {
                int x = memory[current + 1];

                if ((modes[0] == 0 ? memory[x] : x) == 0)
                {
                    int y = memory[current + 2];
                    current = modes[1] == 0 ? memory[y] : y;
                }
                else
                {
                    current += 3;
                }
            }

            void LessThan(int current, int[] modes)
            {
                int x = memory[current + 1];
                int y = memory[current + 2];
                int z = memory[current + 3];

                int left = modes[0] == 0 ? memory[x] : x;
                int right = modes[1] == 0 ? memory[y] : y;

                memory[z] = left < right ? 1 : 0;
            }

            void Equals(int current, int[] modes)
            {
                int x = memory[current + 1];
                int y = memory[current + 2];
                int z = memory[current + 3];

                int left = modes[0] == 0 ? memory[x] : x;
                int right = modes[1] == 0 ? memory[y] : y;

                memory[z] = left == right ? 1 : 0;
            }

            static (int opcode, int[] modes, int length) Decode(int instruction)
            {
                if (instruction == 99)
                {
                    return (99, Array.Empty<int>(), 1);
                }

                string digits = instruction.ToString(CultureInfo.InvariantCulture);

                int opcode = digits[^1] - '0';
                int parameters = opcode switch
                {
                    1 => 3,
                    2 => 3,
                    3 => 1,
                    4 => 1,
                    5 => 3,
                    6 => 3,
                    7 => 3,
                    8 => 3,
                    _ => throw new InvalidOperationException($"{opcode} is not a supported opcode."),
                };

                int[] modes = new int[parameters];
                int j = 0;

                for (int i = digits.Length - 3; i >= 0; i--, j++)
                {
                    modes[j] = digits[i] - '0';
                }

                int length = opcode switch
                {
                    5 => 0,
                    6 => 0,
                    _ => parameters + 1,
                };

                return (opcode, modes, length);
            }

            for (int i = 0; i < memory.Length;)
            {
                (int opcode, int[] modes, int length) = Decode(memory[i]);

                if (opcode == 99)
                {
                    break;
                }

                switch (opcode)
                {
                    case 1:
                        Add(i, modes);
                        break;

                    case 2:
                        Multiply(i, modes);
                        break;

                    case 3:
                        if (!enumerator.MoveNext())
                        {
                            throw new InvalidOperationException();
                        }

                        Input(i, enumerator.Current);
                        break;

                    case 4:
                        output = Output(i, modes);
                        break;

                    case 5:
                        JumpIfTrue(ref i, modes);
                        break;

                    case 6:
                        JumpIfFalse(ref i, modes);
                        break;

                    case 7:
                        LessThan(i, modes);
                        break;

                    case 8:
                        Equals(i, modes);
                        break;

                    default:
                        throw new InvalidOperationException($"{opcode} is not a supported opcode.");
                }

                i += length;
            }

            return memory;
        }
    }
}
