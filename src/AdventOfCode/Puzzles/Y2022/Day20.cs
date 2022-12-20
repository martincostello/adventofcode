// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/20</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 20, "Grove Positioning System", RequiresData = true)]
public sealed class Day20 : Puzzle
{
    /// <summary>
    /// Gets the sum of the three numbers that form the grove coordinates.
    /// </summary>
    public int SumOfCoordinates { get; private set; }

    /// <summary>
    /// Finds the grove coordinates from the specified encrypted file.
    /// </summary>
    /// <param name="values">The values to decrypt to find the grove.</param>
    /// <returns>
    /// The sum of the three numbers that form the grove coordinates.
    /// </returns>
    public static int FindGrove(IList<string> values)
    {
        int[] unmixed = values.Select(Parse<int>).ToArray();
        var mixed = Mix(unmixed);

        int indexOf0 = mixed.IndexOf(0);

        return
            mixed[(indexOf0 + 1000) % values.Count] +
            mixed[(indexOf0 + 2000) % values.Count] +
            mixed[(indexOf0 + 3000) % values.Count];

        static IList<int> Mix(int[] values)
        {
            var mixed = new LinkedList<int>(values);

            for (int i = 0; i < values.Length; i++)
            {
                int value = values[i];
                mixed.Move(value, value);
            }

            return mixed.ToArray();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync();

        SumOfCoordinates = FindGrove(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the three numbers that form the grove coordinates is {0}.", SumOfCoordinates);
        }

        return PuzzleResult.Create(SumOfCoordinates);
    }
}
