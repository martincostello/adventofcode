// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 17, "No Such Thing as Too Much", MinimumArguments = 1, RequiresData = true)]
public sealed class Day17 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the combinations of containers that can be used to completely fill
    /// one or more containers completely with the specified total volume of eggnog.
    /// </summary>
    /// <param name="volume">The volume of eggnog.</param>
    /// <param name="containerVolumes">The volumes of the containers.</param>
    /// <returns>
    /// The combinations of containers that can store the volume specified by <paramref name="volume"/>.
    /// </returns>
    internal static IList<ICollection<int>> GetContainerCombinations(int volume, IList<int> containerVolumes)
    {
        var containers = containerVolumes
            .OrderDescending()
            .ToList();

        return Maths.GetCombinations(volume, containers);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var containerVolumes = await ReadResourceAsNumbersAsync<int>(cancellationToken);

        int volume = Parse<int>(args[0]);

        var combinations = GetContainerCombinations(volume, containerVolumes);

        var combinationsWithLeastContainers = combinations
            .CountBy((p) => p.Count)
            .OrderBy((p) => p.Key)
            .First();

        Solution1 = combinations.Count;
        Solution2 = combinationsWithLeastContainers.Value;

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog.",
                Solution1,
                volume);

            Logger.WriteLine(
                "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog using {2} containers.",
                Solution2,
                volume,
                combinationsWithLeastContainers.Key);
        }

        return Result();
    }
}
