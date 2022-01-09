// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 07, "The Sum of Its Parts", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the order in which the parts of the sleigh should be assembled.
    /// </summary>
    public string OrderOfAssembly { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the time, in seconds, it takes to assemble the sleigh.
    /// </summary>
    public int TimeToAssemble { get; private set; }

    /// <summary>
    /// Assembles the sleigh from the specified instructions.
    /// </summary>
    /// <param name="instructions">The assembly instructions.</param>
    /// <param name="partDuration">The amount of time, in seconds, each part takes to assemble.</param>
    /// <param name="workers">How many workers can assembly the sleigh in parallel.</param>
    /// <returns>
    /// The order in which the sleigh should be assembled and the time,
    /// in seconds, it takes to assemble the sleigh.
    /// </returns>
    public static (string OrderOfAssembly, int TimeToAssemble) Assemble(
        IEnumerable<string> instructions,
        int partDuration,
        int workers)
    {
        var available = new HashSet<string>();
        var constraints = new Dictionary<string, HashSet<string>>();

        foreach (string instruction in instructions)
        {
            string constraint = instruction["Step ".Length..];
            string antecedent = constraint[..1];

            constraint = constraint[(" must be finished before step ".Length + 1)..];

            string part = constraint[..1];

            constraints.GetOrAdd(part).Add(antecedent);

            available.Add(part);
            available.Add(antecedent);
        }

        var inProgress = new Dictionary<string, int>();
        var used = new List<string>();

        int timeToAssemble = 0;

        while (available.Count > 0)
        {
            foreach (string part in inProgress.Keys)
            {
                inProgress[part]--;
            }

            foreach (string part in inProgress.Where((p) => p.Value == 0).Select((p) => p.Key))
            {
                inProgress.Remove(part);
                used.Add(part);
            }

            foreach (string part in available.OrderBy((p) => p))
            {
                bool ready =
                    inProgress.Count < workers &&
                    (!constraints.TryGetValue(part, out var antecedents) ||
                     antecedents.IsSubsetOf(used));

                if (ready && workers > 0)
                {
                    available.Remove(part);
                    inProgress.Add(part, partDuration + (part[0] - 'A') + 1);
                }
            }

            timeToAssemble++;
        }

        var remaining = inProgress.Single();

        used.Add(remaining.Key);
        timeToAssemble += remaining.Value - 1;

        string orderOfAssembly = string.Join(string.Empty, used);

        return (orderOfAssembly, timeToAssemble);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync();

        const int OneMinute = 60;

        (OrderOfAssembly, _) = Assemble(instructions, workers: 1, partDuration: OneMinute);
        (_, TimeToAssemble) = Assemble(instructions, workers: 5, partDuration: OneMinute);

        if (Verbose)
        {
            Logger.WriteLine("The order of assembly for the sleigh with one worker is {0}.", OrderOfAssembly);
            Logger.WriteLine("The time to assemble the sleigh with 5 workers is {0}.", TimeToAssemble);
        }

        return PuzzleResult.Create(OrderOfAssembly, TimeToAssemble);
    }
}
