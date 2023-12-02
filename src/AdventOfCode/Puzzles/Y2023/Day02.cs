// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/02</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 02, "Cube Conundrum", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// Gets the sum of the IDs of the games that are possible.
    /// </summary>
    public int SumOfPossibleSolutions { get; private set; }

    /// <summary>
    /// Determines the sum of the IDs of the specified games that are possible.
    /// </summary>
    /// <param name="values">The games of Cube Conundrum to play.</param>
    /// <returns>
    /// The sum of the IDs of the games that are possible.
    /// </returns>
    public static int Solve(IList<string> values)
    {
        var possible = new List<int>();

        foreach (string game in values)
        {
            string[] split = game.Split(':');
            string[] rounds = split[1].Split(';');

            int blue = 0;
            int green = 0;
            int red = 0;

            foreach (string round in rounds)
            {
                string[] cubes = round.Split(',');

                foreach (string cube in cubes)
                {
                    (string count, string color) = cube.TrimStart().Bifurcate(' ');
                    int number = Parse<int>(count);

                    switch (color)
                    {
                        case "blue":
                            blue = Math.Max(number, blue);
                            break;

                        case "green":
                            green = Math.Max(number, green);
                            break;

                        case "red":
                            red = Math.Max(number, red);
                            break;

                        default:
                            throw new PuzzleException($"Unknown color '{color}'.");
                    }
                }
            }

            if (blue <= 14 && green <= 13 && red <= 12)
            {
                int id = Parse<int>(split[0]["Game ".Length..]);
                possible.Add(id);
            }
        }

        return possible.Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfPossibleSolutions = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the IDs of the possible games is {0}.", SumOfPossibleSolutions);
        }

        return PuzzleResult.Create(SumOfPossibleSolutions);
    }
}
