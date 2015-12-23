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
    /// A class representing the puzzle for <c>http://adventofcode.com/day/23</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day23 : IPuzzle
    {
        /// <summary>
        /// Gets the final value of the <c>a</c> register.
        /// </summary>
        internal int A { get; private set; }

        /// <summary>
        /// Gets the final value of the <c>b</c> register.
        /// </summary>
        internal int B { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1 && args.Length != 2)
            {
                Console.Error.WriteLine("No input file path or start value for register a specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            string[] instructions = File.ReadAllLines(args[0]);
            int initialValue = args.Length == 2 ? int.Parse(args[1], CultureInfo.InvariantCulture) : 0;

            Tuple<int, int> result = ProcessInstructions(instructions, initialValue);

            A = result.Item1;
            B = result.Item2;

            Console.WriteLine(
                "After processing {0:N0} instructions, the value of a is {1:N0} and the value of b is {2:N0}.",
                instructions.Length,
                A,
                B);

            return 0;
        }

        /// <summary>
        /// Processes the specified instructions and returns the values of registers a and b.
        /// </summary>
        /// <param name="instructions">The instructions to process.</param>
        /// <param name="initialValue">The initial value to use for register a.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that contains the values of the a and b registers.
        /// </returns>
        internal static Tuple<int, int> ProcessInstructions(IList<string> instructions, int initialValue)
        {
            Register a = new Register()
            {
                Value = initialValue,
            };

            Register b = new Register();

            for (int i = 0; i >= 0 && i < instructions.Count;)
            {
                string instruction = instructions[i];
                string[] split = instruction.Split(' ');

                string operation = split.ElementAtOrDefault(0);
                string registerOrOffset = split.ElementAtOrDefault(1);
                string offset = split.ElementAtOrDefault(2);

                int next = i + 1;

                switch (operation)
                {
                    case "hlf":
                        (registerOrOffset == "a" ? a : b).Value /= 2;
                        break;

                    case "tpl":
                        (registerOrOffset == "a" ? a : b).Value *= 3;
                        break;

                    case "inc":
                        (registerOrOffset == "a" ? a : b).Value++;
                        break;

                    case "jmp":
                        next = i + int.Parse(registerOrOffset, CultureInfo.InvariantCulture);
                        break;

                    case "jie":
                        if ((registerOrOffset.Split(',')[0].Trim() == "a" ? a : b).Value % 2 == 0)
                        {
                            next = i + int.Parse(offset, CultureInfo.InvariantCulture);
                        }

                        break;

                    case "jio":
                        if ((registerOrOffset.Split(',')[0].Trim() == "a" ? a : b).Value == 1)
                        {
                            next = i + int.Parse(offset, CultureInfo.InvariantCulture);
                        }

                        break;

                    default:
                        Console.Error.WriteLine("Instruction '{0}' is not defined.", operation);
                        return Tuple.Create(-1, -1);
                }

                if (next == i)
                {
                    Console.Error.WriteLine("Instruction at line {0:N0} creates an infinite loop.", i + 1);
                    break;
                }

                i = next;
            }

            return Tuple.Create(a.Value, b.Value);
        }

        /// <summary>
        /// A class representing a processor register. This class cannot be inherited.
        /// </summary>
        private sealed class Register
        {
            /// <summary>
            /// Gets or sets the value of the register.
            /// </summary>
            internal int Value { get; set; }
        }
    }
}
