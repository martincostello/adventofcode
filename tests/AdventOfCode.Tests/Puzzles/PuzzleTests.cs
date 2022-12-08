// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace MartinCostello.AdventOfCode.Puzzles;

public static class PuzzleTests
{
    [Fact]
    public static void Puzzle_Metadata_Is_Correct()
    {
        // Arrange
        var allPuzzles = typeof(Puzzle).Assembly
            .GetTypes()
            .Where((p) => p.IsAssignableTo(typeof(Puzzle)))
            .Select((p) => p.GetCustomAttribute<PuzzleAttribute>()!)
            .Where((p) => p is not null)
            .ToList();

        // Act and Assert
        foreach (var puzzlesForYear in allPuzzles.GroupBy((p) => p.Year))
        {
            var days = puzzlesForYear.Select((p) => p.Day).ToList();
            days.ShouldBe(days.Distinct(), $"{puzzlesForYear.Key} contains duplicate days.");
        }

        var names = allPuzzles.Select((p) => p.Name).ToList();
        names.ShouldBe(names.Distinct(), "Puzzle names are not unique.");
    }
}
