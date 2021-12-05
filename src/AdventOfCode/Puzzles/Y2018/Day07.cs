// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 07, RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the order in which the parts of the sleigh should be assemblied.
    /// </summary>
    public string OrderOfAssembly { get; private set; } = string.Empty;

    /// <summary>
    /// Assembles the sleigh from the specified instructions.
    /// </summary>
    /// <param name="instructions">The assembly instructions.</param>
    /// <returns>
    /// The order in which the sleigh should be assembled.
    /// </returns>
    public static string Assemble(IEnumerable<string> instructions)
    {
        var available = new HashSet<string>();
        var used = new List<string>();
        var constraints = new Dictionary<string, HashSet<string>>();

        foreach (string instruction in instructions)
        {
            string constraint = instruction["Step ".Length..];
            string antecedent = constraint[..1];

            constraint = constraint[(" must be finished before step ".Length + 1)..];

            string part = constraint[..1];

            if (!constraints.TryGetValue(part, out var antecedents))
            {
                antecedents = constraints[part] = new HashSet<string>();
            }

            constraints[part].Add(antecedent);

            available.Add(part);
            available.Add(antecedent);
        }

        while (available.Count > 0)
        {
            foreach (string part in available.OrderBy((p) => p))
            {
                bool ready =
                    !constraints.TryGetValue(part, out var antecedents) ||
                    antecedents.IsSubsetOf(used);

                if (ready)
                {
                    available.Remove(part);
                    used.Add(part);
                    break;
                }
            }
        }

        return string.Join(string.Empty, used);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync();

        OrderOfAssembly = Assemble(instructions);

        if (Verbose)
        {
            Logger.WriteLine("The order of assembly for the sleigh is {0}.", OrderOfAssembly);
        }

        return PuzzleResult.Create(OrderOfAssembly);
    }
}
