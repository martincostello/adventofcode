﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/17</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 17, MinimumArguments = 1)]
    public sealed class Day17 : Puzzle
    {
        /// <summary>
        /// Gets the shortest path to the vault.
        /// </summary>
        public string ShortestPathToVault { get; private set; } = string.Empty;

        /// <summary>
        /// Determines the shortest path to reach the vault.
        /// </summary>
        /// <param name="passcode">The passcode to use.</param>
        /// <returns>
        /// The shortest path that can be taken to reach the vault.
        /// </returns>
        public static string GetShortestPathToVault(string passcode)
        {
            var up = (vector: new Size(0, -1), direction: 'U');
            var down = (vector: new Size(0, 1), direction: 'D');
            var left = (vector: new Size(-1, 0), direction: 'L');
            var right = (vector: new Size(1, 0), direction: 'R');

            var possibleMoves = new[] { up, down, left, right };

            var vault = new Point(3, 3);

            var path = new Stack<char>();
            var routes = new List<string>();

            GetPathsToVault(Point.Empty, path, routes);

            return routes
                .OrderBy((p) => p.Length)
                .FirstOrDefault() ?? string.Empty;

            void GetPathsToVault(Point current, Stack<char> path, ICollection<string> routes)
            {
                string pathSoFar = string.Join(string.Empty, path.Reverse());

                if (current == vault)
                {
                    routes.Add(pathSoFar);
                    return;
                }

                if (routes.Count > 0 &&
                    pathSoFar.Length > routes.Min((p) => p.Length))
                {
                    // We cannot find a better path so stop searching
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
                    'b' => true,
                    'c' => true,
                    'd' => true,
                    'e' => true,
                    'f' => true,
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

            ShortestPathToVault = GetShortestPathToVault(passcode);

            if (Verbose)
            {
                Logger.WriteLine("The shortest path to the vault is {0}.", ShortestPathToVault);
            }

            return PuzzleResult.Create(ShortestPathToVault);
        }
    }
}
