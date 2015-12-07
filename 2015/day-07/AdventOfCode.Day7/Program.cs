// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/7</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            IList<string> instructions = File.ReadAllLines(args[0]);

            // Get the wire values for the initial instructions
            IDictionary<string, ushort> values = GetWireValues(instructions);

            ushort a = values["a"];

            Console.WriteLine("The signal for wire a is {0:N0}.", a);

            // Replace the input value for b with the value for a, then re-calculate
            int indexForB = instructions.IndexOf("44430 -> b");
            instructions[indexForB] = string.Format(CultureInfo.InvariantCulture, "{0} -> b", a);

            values = GetWireValues(instructions);

            a = values["a"];

            Console.WriteLine("The new signal for wire a is {0:N0}.", a);

            return 0;
        }

        /// <summary>
        /// Gets the wire values for the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions to get the wire values for.</param>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> containing the values for wires keyed by their Ids.</returns>
        internal static IDictionary<string, ushort> GetWireValues(IEnumerable<string> instructions)
        {
            // Create a map of wire Ids to the instructions to get their value
            var instructionMap = instructions
                .Select((p) => p.Split(new string[] { " -> " }, StringSplitOptions.None))
                .ToDictionary((p) => p.Last(), (p) => p.First().Split(' '));

            Dictionary<string, ushort> result = new Dictionary<string, ushort>();

            // Loop through the instructions until we have reduced each instruction to a value
            while (result.Count != instructionMap.Count)
            {
                foreach (var pair in instructionMap)
                {
                    string wireId = pair.Key;

                    if (result.ContainsKey(wireId))
                    {
                        // We already have the value for this wire
                        continue;
                    }

                    string[] words = pair.Value;
                    ushort? solvedValue = null;

                    string firstOperand = words.FirstOrDefault();
                    string secondOperand;

                    if (words.Length == 1)
                    {
                        // "123 -> x" or " -> "lx -> a"
                        ushort value;

                        // Is the instruction a value or a previously solved value?
                        if (ushort.TryParse(firstOperand, out value) || result.TryGetValue(firstOperand, out value))
                        {
                            result[wireId] = value;
                        }
                    }
                    else if (words.Length == 2 && firstOperand == "NOT")
                    {
                        // "NOT e -> f" or "NOT 1 -> g"
                        secondOperand = words.ElementAtOrDefault(1);

                        ushort value;

                        // Is the second operand a value or a previously solved value?
                        if (ushort.TryParse(secondOperand, out value) ||
                            result.TryGetValue(secondOperand, out value))
                        {
                            result[wireId] = (ushort)~value;
                        }
                    }
                    else if (words.Length == 3)
                    {
                        secondOperand = words.ElementAtOrDefault(2);

                        ushort firstValue;
                        ushort secondValue;

                        // Are both operands a value or a previously solved value?
                        if ((ushort.TryParse(firstOperand, out firstValue) || result.TryGetValue(firstOperand, out firstValue)) &&
                            (ushort.TryParse(secondOperand, out secondValue) || result.TryGetValue(secondOperand, out secondValue)))
                        {
                            string operation = words.ElementAtOrDefault(1);

                            if (operation == "AND")
                            {
                                // "x AND y -> z"
                                solvedValue = (ushort)(firstValue & secondValue);
                            }
                            else if (operation == "OR")
                            {
                                // "i OR j => k"
                                solvedValue = (ushort)(firstValue | secondValue);
                            }
                            else if (operation == "LSHIFT")
                            {
                                // "p LSHIFT 2"
                                solvedValue = (ushort)(firstValue << secondValue);
                            }
                            else if (operation == "RSHIFT")
                            {
                                // "q RSHIFT 3"
                                solvedValue = (ushort)(firstValue >> secondValue);
                            }
                        }
                    }

                    // The value for this wire Id has been solved
                    if (solvedValue.HasValue)
                    {
                        result[wireId] = solvedValue.Value;
                    }
                }
            }

            return result;
        }
    }
}
