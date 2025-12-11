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
    internal static IList<ICollection<int>> GetContainerCombinations(int volume, List<int> containerVolumes)
    {
        var containers = containerVolumes
            .OrderDescending()
            .ToList();

        return Maths.GetCombinations(volume, containers);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithNumbersAsync<int>(
            args,
            static (arguments, containerVolumes, logger, _) =>
            {
                int volume = Parse<int>(arguments[0]);

                var allCombinations = GetContainerCombinations(volume, containerVolumes);

                var combinationsWithLeastContainers = allCombinations
                    .CountBy((p) => p.Count)
                    .OrderBy((p) => p.Key)
                    .First();

                int combinations = allCombinations.Count;
                int combinationsWithMinimumContainers = combinationsWithLeastContainers.Value;

                if (logger is { })
                {
                    logger.WriteLine(
                        "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog.",
                        combinations,
                        volume);

                    logger.WriteLine(
                        "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog using {2} containers.",
                        combinationsWithMinimumContainers,
                        volume,
                        combinationsWithLeastContainers.Key);
                }

                return (combinations, combinationsWithMinimumContainers);
            },
            cancellationToken);
    }
}
