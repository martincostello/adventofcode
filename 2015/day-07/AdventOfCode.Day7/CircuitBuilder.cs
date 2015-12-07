// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircuitBuilder.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   CircuitBuilder.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class that builds circuits of logic gates from text instructions. This class cannot be inherited.
    /// </summary>
    internal static class CircuitBuilder
    {
        /// <summary>
        /// Returns an <see cref="IDictionary{TKey, TValue}"/> representing a circuit of
        /// logic gates created from the specified set of textual instruction(s).
        /// </summary>
        /// <param name="instructions">A collection of textual instructions.</param>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> representing the circuit created from <paramref name="instructions"/>.</returns>
        internal static IDictionary<string, ILogicGate> Build(IEnumerable<string> instructions)
        {
            Dictionary<string, ILogicGate> circuit = new Dictionary<string, ILogicGate>();

            foreach (string instruction in instructions)
            {
                Console.WriteLine(instruction);

                string[] split = instruction.Split(new[] { " -> " }, StringSplitOptions.None);

                string id = split.ElementAtOrDefault(1);
                string operation = split.ElementAtOrDefault(0);

                split = operation.Split(' ');

                string first = split.ElementAtOrDefault(0);

                ILogicGate gate = null;

                if (split.Length == 1)
                {
                    ushort signal;

                    if (TryParseSignal(first, out signal))
                    {
                        // "123 => x"
                        gate = new SimpleGate(id, signal);
                    }
                    else
                    {
                        // "a -> b"
                        gate = new FutureGate(id, () => circuit[first].Signal);
                    }
                }
                else if (split.Length == 2)
                {
                    // "NOT x -> h"
                    string otherId = split.ElementAtOrDefault(1);
                    gate = new NotGate(id, circuit[otherId]);
                }
                else if (split.Length == 3)
                {
                    switch (split[1])
                    {
                        // "x AND y -> d"
                        case "AND":
                            gate = new AndGate(id, circuit[first], circuit[split.Last()]);
                            break;

                        // "x OR y -> e"
                        case "OR":
                            gate = new OrGate(id, circuit[first], circuit[split.Last()]);
                            break;

                        // "x LSHIFT 2 -> f"
                        case "LSHIFT":
                            gate = new LeftShiftGate(id, circuit[first], ParseSignal(split.Last()));
                            break;

                        // "y RSHIFT 2 -> g"
                        case "RSHIFT":
                            gate = new RightShiftGate(id, circuit[first], ParseSignal(split.Last()));
                            break;

                        default:
                            break;
                    }
                }

                if (id == null || gate == null)
                {
                    throw new ArgumentException($"An invalid instruction was specified: '{instruction}'.", nameof(instructions));
                }

                circuit[id] = gate;
            }

            return circuit;
        }

        /// <summary>
        /// Parses the specified <see cref="string"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <param name="s">The value to parse.</param>
        /// <returns>The parsed value.</returns>
        private static ushort ParseSignal(string s) => ushort.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);

        /// <summary>
        /// Tries to parse the specified <see cref="string"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <param name="s">The value to parse.</param>
        /// <param name="value">The parsed value of <paramref name="s"/>, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise <see langword="false"/>.</returns>
        private static bool TryParseSignal(string s, out ushort value) => ushort.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }
}
