// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 05, "Supply Stacks", RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets a string containing the IDs of the crates at the top of each stack.
    /// </summary>
    public string TopCratesOfStacks { get; private set; } = string.Empty;

    /// <summary>
    /// Rearranges the crates specified by the puzzle input.
    /// </summary>
    /// <param name="instructions">The instructions of how to rearrange the crates.</param>
    /// <returns>
    /// A string containing the IDs of the crates at the top of each stack after rearrangement.
    /// </returns>
    public static string RearrangeCrates(IList<string> instructions)
    {
        int count = GetStackCount(instructions);
        var stacks = GetStacks(instructions, count);
        var steps = GetSteps(instructions);

        Rearrange(steps, stacks);

        return GetTopCrates(stacks);

        static int GetStackCount(IList<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                string line = instructions[i];

                if (string.IsNullOrEmpty(line))
                {
                    string trimmed = instructions[i - 1].Trim();
                    return Parse<int>(trimmed.Split(' ').Last());
                }
            }

            return -1;
        }

        static List<Stack<char>> GetStacks(IList<string> instructions, int count)
        {
            var stacks = new List<Stack<char>>(count);

            for (int i = 0; i < count; i++)
            {
                stacks.Add(new Stack<char>());
            }

            foreach (string line in instructions.Take(count).Reverse())
            {
                for (int i = 1, j = 0; i < line.Length; i += 4, j++)
                {
                    char container = line[i];

                    if (container is ' ')
                    {
                        continue;
                    }

                    stacks[j].Push(container);
                }
            }

            return stacks;
        }

        static List<(int Count, int From, int To)> GetSteps(IList<string> instructions)
        {
            var result = new List<(int Count, int From, int To)>();

            foreach (string instruction in instructions.SkipWhile((p) => !string.IsNullOrEmpty(p)).Skip(1))
            {
                string[] split = instruction.Split(' ');
                result.Add((Parse<int>(split[1]), Parse<int>(split[3]), Parse<int>(split[5])));
            }

            return result;
        }

        static string GetTopCrates(List<Stack<char>> stacks)
        {
            var result = new StringBuilder(stacks.Count);

            foreach (var stack in stacks)
            {
                result.Append(stack.Pop());
            }

            return result.ToString();
        }

        static void Rearrange(List<(int Count, int From, int To)> steps, List<Stack<char>> stacks)
        {
            foreach ((int count, int from, int to) in steps)
            {
                for (int i = 0; i < count; i++)
                {
                    stacks[to - 1].Push(stacks[from - 1].Pop());
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var instructions = await ReadResourceAsLinesAsync();

        TopCratesOfStacks = RearrangeCrates(instructions);

        if (Verbose)
        {
            Logger.WriteLine("The crates on the top of each stack are: {0}.", TopCratesOfStacks);
        }

        return PuzzleResult.Create(TopCratesOfStacks);
    }
}
