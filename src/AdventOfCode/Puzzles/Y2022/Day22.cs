// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 22, "Monkey Map", RequiresData = true, IsHidden = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the final password.
    /// </summary>
    public int FinalPassword { get; private set; }

    /// <summary>
    /// Gets the final password by parsing the specified map.
    /// </summary>
    /// <param name="map">The map to parse.</param>
    /// <returns>
    /// The final password derived from <paramref name="map"/>.
    /// </returns>
    public static int GetFinalPassword(IList<string> map)
    {
        var directions = ParseDirections('f' + map[^1]);

        return directions.Count;

        static List<(int Units, char Direction)> ParseDirections(ReadOnlySpan<char> directions)
        {
            var result = new List<(int Units, char Direction)>();

            for (int i = 1; i < directions.Length; i++)
            {
                if (!char.IsDigit(directions[i]))
                {
                    int units = Parse<int>(directions[1..i]);
                    char direction = directions[0];

                    result.Add((units, direction));

                    directions = directions[(i + 1)..];
                    i = -1;
                }
            }

            return result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync();

        FinalPassword = GetFinalPassword(values);

        if (Verbose)
        {
            Logger.WriteLine("The final password is {0}.", FinalPassword);
        }

        return PuzzleResult.Create(FinalPassword);
    }
}
