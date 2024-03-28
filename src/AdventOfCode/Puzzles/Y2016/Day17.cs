// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 17, "Two Steps Forward", MinimumArguments = 1)]
public sealed class Day17 : Puzzle
{
    /// <summary>
    /// Gets the shortest path to the vault.
    /// </summary>
    public string ShortestPathToVault { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the longest path to the vault.
    /// </summary>
    public int LongestPathToVault { get; private set; }

    /// <summary>
    /// Determines the shortest path to reach the vault.
    /// </summary>
    /// <param name="passcode">The passcode to use.</param>
    /// <returns>
    /// The shortest path that can be taken to reach the vault and the length of the longest path.
    /// </returns>
    public static (string Shortest, int Longest) GetPathsToVault(string passcode)
    {
        var up = (vector: Directions.Up, direction: 'U');
        var down = (vector: Directions.Down, direction: 'D');
        var left = (vector: Directions.Left, direction: 'L');
        var right = (vector: Directions.Right, direction: 'R');

        var possibleMoves = new[] { up, down, left, right };

        var vault = new Point(3, 3);

        var path = new Stack<char>();
        var routes = new List<string>();

        GetPathsToVault(Point.Empty, path, routes);

        string shortestPath = routes
            .OrderBy((p) => p.Length)
            .FirstOrDefault() ?? string.Empty;

        int longestPath = routes
            .Select((p) => p.Length)
            .OrderDescending()
            .FirstOrDefault();

        return (shortestPath, longestPath);

        void GetPathsToVault(Point current, Stack<char> path, List<string> routes)
        {
            char[] reversed = [.. path];
            Array.Reverse(reversed);

            string pathSoFar = new(reversed);

            if (current == vault)
            {
                routes.Add(pathSoFar);
                return;
            }

            string hash = Hash(passcode + pathSoFar);

            for (int i = 0; i < possibleMoves.Length; i++)
            {
                var (vector, direction) = possibleMoves[i];
                var candidate = current + vector;

                if (CanUseDoor(hash[i], candidate))
                {
                    path.Push(direction);

                    GetPathsToVault(candidate, path, routes);

                    path.Pop();
                }
            }
        }

        static string Hash(string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            byte[] hash = MD5.HashData(buffer);

#pragma warning disable CA1308
            return Convert.ToHexString(hash)[0..4].ToLowerInvariant();
#pragma warning restore CA1308
        }

        static bool IsOpenDoor(char value)
        {
            return value switch
            {
                'b' or 'c' or 'd' or 'e' or 'f' => true,
                _ => false,
            };
        }

        static bool IsValidMove(Point location)
            => location.X > -1 &&
               location.Y > -1 &&
               location.X < 4 &&
               location.Y < 4;

        static bool CanUseDoor(char passcode, Point location)
            => IsOpenDoor(passcode) && IsValidMove(location);
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string passcode = args[0];

        (ShortestPathToVault, LongestPathToVault) = GetPathsToVault(passcode);

        if (Verbose)
        {
            Logger.WriteLine("The shortest path to the vault is {0}.", ShortestPathToVault);
            Logger.WriteLine("The longest path to the vault is {0}.", LongestPathToVault);
        }

        return PuzzleResult.Create(ShortestPathToVault, LongestPathToVault);
    }
}
