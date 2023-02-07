// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/25</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 25, "Sea Cucumber", RequiresData = true)]
public sealed class Day25 : Puzzle
{
    /// <summary>
    /// Gets the number of sea cucumber moves after which no further moves are made.
    /// </summary>
    public int FirstStepWithNoMoves { get; private set; }

    /// <summary>
    /// Clears the sea floor of sea cucumbers to create a space to land the submarine.
    /// </summary>
    /// <param name="map">The map of the sea floor.</param>
    /// <returns>
    /// The number of sea cucumber moves after which no further moves are made.
    /// </returns>
    public static int ClearSeaFloor(IList<string> map)
    {
        int width = map[0].Length;
        int height = map.Count;

        char[,] seaFloor = new char[width, height];

        for (int y = 0; y < height; y++)
        {
            string row = map[y];

            for (int x = 0; x < width; x++)
            {
                seaFloor[x, y] = row[x];
            }
        }

        int steps = 0;

        do
        {
            steps++;
        }
        while (Step(seaFloor, width, height));

        return steps;

        static bool Step(char[,] seaFloor, int width, int height)
        {
            const char East = '>';
            const char South = 'v';

            bool moved = Move(East, seaFloor, width, height);
            moved |= Move(South, seaFloor, width, height);

            return moved;

            static bool Move(char candidate, char[,] seaFloor, int width, int height)
            {
                const char Empty = '.';

                var from = new List<(int X, int Y)>();
                var to = new List<(int X, int Y)>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        char square = seaFloor[x, y];

                        if (square != candidate)
                        {
                            continue;
                        }

                        (int X, int Y) destination;

                        if (square == East)
                        {
                            destination = (x + 1, y);

                            if (destination.X == width)
                            {
                                destination = (0, y);
                            }
                        }
                        else
                        {
                            destination = (x, y + 1);

                            if (destination.Y == height)
                            {
                                destination = (x, 0);
                            }
                        }

                        char target = seaFloor[destination.X, destination.Y];

                        if (target == Empty)
                        {
                            from.Add((x, y));
                            to.Add(destination);
                        }
                    }
                }

                for (int i = 0; i < from.Count; i++)
                {
                    var (sourceX, sourceY) = from[i];
                    var (destinationX, destinationY) = to[i];

                    seaFloor[destinationX, destinationY] = candidate;
                    seaFloor[sourceX, sourceY] = Empty;
                }

                return from.Count > 0;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync(cancellationToken);

        FirstStepWithNoMoves = ClearSeaFloor(instructions);

        if (Verbose)
        {
            Logger.WriteLine("The first step on which no sea cucumbers move is {0:N0}.", FirstStepWithNoMoves);
        }

        return PuzzleResult.Create(FirstStepWithNoMoves);
    }
}
