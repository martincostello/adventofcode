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
    /// Gets the sum of the power of the set of cubes in each game.
    /// </summary>
    public int SumOfPowers { get; private set; }

    /// <summary>
    /// Plays the specified games of Cube Conundrum and returns
    /// the sum of the IDs of the games that are possible and
    /// the sum of the power of the set of cubes in each game.
    /// </summary>
    /// <param name="values">The games of Cube Conundrum to play.</param>
    /// <returns>
    /// The sum of the IDs of the games that are possible and the
    /// sum of the power of the set of cubes in each game.
    /// </returns>
    public static (int SumOfPossibleSolutions, int SumOfPowers) Play(IList<string> values)
    {
        int sumOfIds = 0;
        int sumOfPowers = 0;

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

            sumOfPowers += blue * green * red;

            if (blue <= 14 && green <= 13 && red <= 12)
            {
                sumOfIds += Parse<int>(split[0]["Game ".Length..]);
            }
        }

        return (sumOfIds, sumOfPowers);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (SumOfPossibleSolutions, SumOfPowers) = Play(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the IDs of the possible games is {0}.", SumOfPossibleSolutions);
            Logger.WriteLine("The sum of the pwoers of the cubes in the games is {0}.", SumOfPowers);
        }

        return PuzzleResult.Create(SumOfPossibleSolutions, SumOfPowers);
    }
}
