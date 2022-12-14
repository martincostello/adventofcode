// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 03, "Rucksack Reorganization", RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Gets the sum of the priorities for the item type
    /// which appears in both compartments of each rucksack.
    /// </summary>
    public int SumOfPriorities { get; private set; }

    /// <summary>
    /// Gets the sum of the priorities for the item type
    /// which appears in all three rucksacks of each group of elves.
    /// </summary>
    public int SumOfPrioritiesOfGroups { get; private set; }

    /// <summary>
    /// Gets the sum of the priorities for the item type which is common to each rucksack.
    /// </summary>
    /// <param name="inventories">The inventories for each rucksack.</param>
    /// <param name="useGroups">Whether to count by groups of elves rather than by compartments.</param>
    /// <returns>
    /// The sum of the priorities for the item type which is common to each rucksack.
    /// </returns>
    public static int GetSumOfCommonItemTypes(IList<string> inventories, bool useGroups)
    {
        if (useGroups)
        {
            int sum = 0;

            for (int i = 0; i < inventories.Count; i += 3)
            {
                string first = inventories[i];
                string second = inventories[i + 1];
                string third = inventories[i + 2];

                char common = GetCommonItemType(first, second, third);
                sum += GetPriority(common);
            }

            return sum;
        }
        else
        {
            return inventories
                .Select((inventory) =>
                {
                    string first = inventory[0..(inventory.Length / 2)];
                    string second = inventory[(inventory.Length / 2)..];
                    return GetCommonItemType(first, second);
                })
                .Select(GetPriority)
                .Sum();
        }

        static char GetCommonItemType(params string[] inventories)
        {
            var intersection = new HashSet<char>(inventories.First());

            foreach (string inventory in inventories.Skip(1))
            {
                intersection.IntersectWith(inventory);
            }

            return intersection.Single();
        }

        static int GetPriority(char item)
            => item - (char.IsUpper(item) ? 'A' - 26 : 'a') + 1;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var inventories = await ReadResourceAsLinesAsync();

        SumOfPriorities = GetSumOfCommonItemTypes(inventories, useGroups: false);
        SumOfPrioritiesOfGroups = GetSumOfCommonItemTypes(inventories, useGroups: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The sum of the priorities of the item types which appear in both compartments is {0:N0}.",
                SumOfPriorities);

            Logger.WriteLine(
                "The sum of the priorities of the item types which appear in all three rucksacks of each group of elves is {0:N0}.",
                SumOfPrioritiesOfGroups);
        }

        return PuzzleResult.Create(SumOfPriorities, SumOfPrioritiesOfGroups);
    }
}
