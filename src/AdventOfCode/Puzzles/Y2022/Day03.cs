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
    /// Gets the sum of the priorities for the item type which appears in both compartments of each rucksack.
    /// </summary>
    /// <param name="inventories">The inventories for each rucksack.</param>
    /// <returns>
    /// The sum of the priorities for the item type which appears in both compartments of each rucksack.
    /// </returns>
    public static int GetSumOfDuplicateItemPriorities(ICollection<string> inventories)
    {
        return inventories
            .Select(GetCommonItemType)
            .Select(GetPriority)
            .Sum();

        static char GetCommonItemType(string inventory)
        {
            var first = new HashSet<char>(inventory[0..(inventory.Length / 2)]);
            var second = new HashSet<char>(inventory[(inventory.Length / 2)..]);
            return first.Intersect(second).Single();
        }

        static int GetPriority(char item)
        {
            if (char.IsUpper(item))
            {
                return item - 'A' + 27;
            }
            else
            {
                return item - 'a' + 1;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var inventories = await ReadResourceAsLinesAsync();

        SumOfPriorities = GetSumOfDuplicateItemPriorities(inventories);

        if (Verbose)
        {
            Logger.WriteLine(
                "The sum of the priorities of the item types which appear in both compartments is {0:N0}.",
                SumOfPriorities);
        }

        return PuzzleResult.Create(SumOfPriorities);
    }
}
