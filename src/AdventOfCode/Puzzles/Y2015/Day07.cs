// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle("Some Assembly Required", 2015, 07, RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the first signal value.
    /// </summary>
    internal int FirstSignal { get; private set; }

    /// <summary>
    /// Gets the second signal value.
    /// </summary>
    internal int SecondSignal { get; private set; }

    /// <summary>
    /// Gets the wire values for the specified instructions.
    /// </summary>
    /// <param name="instructions">The instructions to get the wire values for.</param>
    /// <returns>
    /// An <see cref="IDictionary{TKey, TValue}"/> containing the values for wires keyed by their Ids.
    /// </returns>
    internal static IDictionary<string, ushort> GetWireValues(IEnumerable<string> instructions)
    {
        // Create a map of wire Ids to the instructions to get their value
        var instructionMap = instructions
            .Select((p) => p.Split(" -> "))
            .ToDictionary((p) => p[^1], (p) => p[0].Split(' '));

        var result = new Dictionary<string, ushort>();

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

                string? firstOperand = words.FirstOrDefault();
                string? secondOperand;

                if (words.Length == 1)
                {
                    // "123 -> x" or " -> "lx -> a"
                    // Is the instruction a value or a previously solved value?
                    if (ushort.TryParse(firstOperand, out ushort value) || result.TryGetValue(firstOperand!, out value))
                    {
                        result[wireId] = value;
                    }
                }
                else if (words.Length == 2 && firstOperand == "NOT")
                {
                    // "NOT e -> f" or "NOT 1 -> g"
                    secondOperand = words.ElementAtOrDefault(1);

                    // Is the second operand a value or a previously solved value?
                    if (ushort.TryParse(secondOperand, out ushort value) ||
                        result.TryGetValue(secondOperand!, out value))
                    {
                        result[wireId] = (ushort)~value;
                    }
                }
                else if (words.Length == 3)
                {
                    secondOperand = words.ElementAtOrDefault(2);

                    // Are both operands a value or a previously solved value?
                    if ((ushort.TryParse(firstOperand, out ushort firstValue) || result.TryGetValue(firstOperand!, out firstValue)) &&
                        (ushort.TryParse(secondOperand, out ushort secondValue) || result.TryGetValue(secondOperand!, out secondValue)))
                    {
                        string? operation = words.ElementAtOrDefault(1);
                        solvedValue = TrySolveValueForOperation(operation!, firstValue, secondValue);
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

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        // Get the wire values for the initial instructions
        IDictionary<string, ushort> values = GetWireValues(instructions);

        FirstSignal = values["a"];

        if (Verbose)
        {
            Logger.WriteLine("The signal for wire a is {0:N0}.", FirstSignal);
        }

        // Replace the input value for b with the value for a, then re-calculate
        int indexForB = instructions.IndexOf("44430 -> b");
        instructions[indexForB] = Format("{0} -> b", FirstSignal);

        values = GetWireValues(instructions);

        SecondSignal = values["a"];

        if (Verbose)
        {
            Logger.WriteLine("The new signal for wire a is {0:N0}.", SecondSignal);
        }

        return PuzzleResult.Create(FirstSignal, SecondSignal);
    }

    /// <summary>
    /// Tries to solve the value for the specified operation and values.
    /// </summary>
    /// <param name="operation">The operation.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="secondValue">The second value.</param>
    /// <returns>
    /// The solved value for the specified parameters if solved; otherwise <see langword="null"/>.
    /// </returns>
    private static ushort? TrySolveValueForOperation(string operation, ushort firstValue, ushort secondValue)
    {
        return operation switch
        {
            "AND" => (ushort)(firstValue & secondValue), // "x AND y -> z"
            "OR" => (ushort)(firstValue | secondValue), // "i OR j => k"
            "LSHIFT" => (ushort)(firstValue << secondValue), // "p LSHIFT 2"
            "RSHIFT" => (ushort)(firstValue >> secondValue), // "q RSHIFT 3"
            _ => null,
        };
    }
}
