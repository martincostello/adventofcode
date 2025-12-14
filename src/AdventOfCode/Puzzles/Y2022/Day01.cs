// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 01, "Calorie Counting", RequiresData = true)]
public sealed class Day01 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the number of calories carried by each elf in the specified inventories.
    /// </summary>
    /// <param name="inventories">The lines of the inventory.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> containing the number of calories carried by each elf.
    /// </returns>
    public static IList<int> GetCalorieInventories(IList<string> inventories)
    {
        var calories = new List<int>();

        int current = 0;

        for (int i = 0; i < inventories.Count; i++)
        {
            string line = inventories[i];

            if (string.IsNullOrEmpty(line))
            {
                calories.Add(current);
                current = 0;
            }
            else
            {
                current += Parse<int>(line);
            }
        }

        if (current > 0)
        {
            calories.Add(current);
        }

        return calories;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (inventories, logger, _) =>
            {
                var calories = GetCalorieInventories(inventories);

                int maximumCalories = calories.Max();
                int maximumCaloriesForTop3 = calories.OrderDescending().Take(3).Sum();

                if (logger is { })
                {
                    logger.WriteLine("The elf carrying the largest inventory has {0:N0} Calories.", maximumCalories);
                    logger.WriteLine("The elves carrying the largest three inventories have {0:N0} Calories.", maximumCaloriesForTop3);
                }

                return (maximumCalories, maximumCaloriesForTop3);
            },
            cancellationToken);
    }
}
