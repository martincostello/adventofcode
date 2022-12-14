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
    /// Gets a string containing the IDs of the crates at
    /// the top of each stack using a CraneMover 9000.
    /// </summary>
    public string TopCratesOfStacks9000 { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a string containing the IDs of the crates at
    /// the top of each stack using a CraneMover 9001.
    /// </summary>
    public string TopCratesOfStacks9001 { get; private set; } = string.Empty;

    /// <summary>
    /// Rearranges the crates specified by the puzzle input.
    /// </summary>
    /// <param name="instructions">The instructions of how to rearrange the crates.</param>
    /// <param name="canMoveMultipleCrates">Whether the crane is capable of moving multiple crates at once.</param>
    /// <returns>
    /// A string containing the IDs of the crates at the top of each stack after rearrangement.
    /// </returns>
    public static string RearrangeCrates(IList<string> instructions, bool canMoveMultipleCrates)
    {
        int count = GetStackCount(instructions);
        var stacks = GetStacks(instructions, count);
        var steps = GetSteps(instructions);

        Rearrange(steps, stacks, canMoveMultipleCrates);

        return GetTopCrates(stacks);

        static int GetStackCount(IList<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                if (string.IsNullOrEmpty(instructions[i]))
                {
                    string trimmed = instructions[i - 1].Trim();
                    return Parse<int>(trimmed[trimmed.LastIndexOf(' ')..]);
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

        static void Rearrange(
            List<(int Count, int From, int To)> steps,
            List<Stack<char>> stacks,
            bool canMoveMultipleCrates)
        {
            Stack<char>? swap = null;

            foreach ((int count, int from, int to) in steps)
            {
                var source = stacks[from - 1];
                var destination = stacks[to - 1];

                if (canMoveMultipleCrates && count > 1)
                {
                    swap ??= new Stack<char>(count);

                    for (int i = 0; i < count; i++)
                    {
                        swap.Push(source.Pop());
                    }

                    for (int i = 0; i < count; i++)
                    {
                        destination.Push(swap.Pop());
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        destination.Push(source.Pop());
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync();

        TopCratesOfStacks9000 = RearrangeCrates(instructions, canMoveMultipleCrates: false);
        TopCratesOfStacks9001 = RearrangeCrates(instructions, canMoveMultipleCrates: true);

        if (Verbose)
        {
            Logger.WriteLine("The crates on the top of each stack with the CraneMover 9000 are: {0}.", TopCratesOfStacks9000);
            Logger.WriteLine("The crates on the top of each stack with the CraneMover 9001 are: {0}.", TopCratesOfStacks9001);
        }

        return PuzzleResult.Create(TopCratesOfStacks9000, TopCratesOfStacks9001);
    }
}
