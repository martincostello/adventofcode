// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2018/day/3</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2018, 03, RequiresData = true)]
    public sealed class Day03 : Puzzle
    {
        /// <summary>
        /// Gets the area, in square inches, of fabric with two or more claims.
        /// </summary>
        public int Area { get; private set; }

        /// <summary>
        /// Gets the Id of the claim with a unique area.
        /// </summary>
        public string? IdOfUniqueClaim { get; private set; }

        /// <summary>
        /// Calculates the number of square inches of fabric with two or more overlapping claims.
        /// </summary>
        /// <param name="claims">The claimed areas of the fabric.</param>
        /// <returns>
        /// The area, in square inches, of fabric with two or more claims specified by <paramref name="claims"/>.
        /// </returns>
        public static int GetAreaWithTwoOrMoreOverlappingClaims(IEnumerable<string> claims)
        {
            (Square[,] fabric, var _) = ParseFabric(claims);

            int result = 0;

            int height = fabric.GetLength(0);
            int width = fabric.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (fabric[i, j].Claims.Count >= 2)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the Id of the claim with no overlapping claims.
        /// </summary>
        /// <param name="claims">The claimed areas of the fabric.</param>
        /// <returns>
        /// The Id of the claim specified by <paramref name="claims"/> that is not overlapped by any others.
        /// </returns>
        public static string? GetClaimWithNoOverlappingClaims(IEnumerable<string> claims)
        {
            (Square[,] fabric, IList<Claim> fabricClaims) = ParseFabric(claims);

            string? result = null;

            foreach (var claim in fabricClaims)
            {
                bool isCandidate = true;

                for (int i = claim.X; i < claim.Width + claim.X && isCandidate; i++)
                {
                    for (int j = claim.Y; j < claim.Height + claim.Y && isCandidate; j++)
                    {
                        var square = fabric[i, j];

                        if (square.Claims.Count != 1 || square.Claims[0] != claim.Id)
                        {
                            isCandidate = false;
                        }
                    }
                }

                if (isCandidate)
                {
                    result = claim.Id;
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> claims = await ReadResourceAsLinesAsync();

            Area = GetAreaWithTwoOrMoreOverlappingClaims(claims);
            IdOfUniqueClaim = GetClaimWithNoOverlappingClaims(claims);

            if (Verbose)
            {
                Logger.WriteLine($"{Area:N0} square inches of fabric are within two or more claims.");
                Logger.WriteLine($"The Id of the claim with no overlapping claims is {IdOfUniqueClaim}.");
            }

            return PuzzleResult.Create(Area, IdOfUniqueClaim!);
        }

        /// <summary>
        /// Parses the grid of fabric from the claims.
        /// </summary>
        /// <param name="claims">The claimed areas of the fabric.</param>
        /// <returns>
        /// A two-dimensional array of squares representing the fabric and the claims for each square inch.
        /// </returns>
        private static (Square[,] fabric, IList<Claim> claims) ParseFabric(IEnumerable<string> claims)
        {
            IList<Claim> fabricClaims = claims
                .Select(Claim.Parse)
                .ToList();

            int totalWidth = fabricClaims.Max((p) => p.X + p.Width);
            int totalHeight = fabricClaims.Max((p) => p.Y + p.Height);

            var fabric = new Square[totalWidth, totalHeight];

            for (int i = 0; i < totalWidth; i++)
            {
                for (int j = 0; j < totalHeight; j++)
                {
                    fabric[i, j] = new Square();
                }
            }

            foreach (var claim in fabricClaims)
            {
                for (int i = claim.X; i < claim.Width + claim.X; i++)
                {
                    for (int j = claim.Y; j < claim.Height + claim.Y; j++)
                    {
                        fabric[i, j].Claims.Add(claim.Id);
                    }
                }
            }

            return (fabric, fabricClaims);
        }

        /// <summary>
        /// A class representing a claim for fabric. This class cannot be inherited.
        /// </summary>
        private sealed class Claim
        {
            /// <summary>
            /// Gets the Id of the claim.
            /// </summary>
            internal string Id { get; private set; } = string.Empty;

            /// <summary>
            /// Gets the number of inches between the left edge of the fabric and the left edge of the rectangle.
            /// </summary>
            internal int X { get; private set; }

            /// <summary>
            /// Gets the number of inches between the top edge of the fabric and the top edge of the rectangle.
            /// </summary>
            internal int Y { get; private set; }

            /// <summary>
            /// Gets the width of the rectangle in inches.
            /// </summary>
            internal int Width { get; private set; }

            /// <summary>
            /// Gets the height of the rectangle in inches.
            /// </summary>
            internal int Height { get; private set; }

            /// <summary>
            /// Parses the specified claim.
            /// </summary>
            /// <param name="claim">The string representation of the claim.</param>
            /// <returns>
            /// The <see cref="Claim"/> represented by <paramref name="claim"/>.
            /// </returns>
            internal static Claim Parse(string claim)
            {
                string[] split = claim.Split(' ');

                string id = split[0].TrimStart('#');
                string offset = split[2].TrimEnd(':');
                string area = split[3];

                split = offset.Split(',');

                int x = ParseInt32(split[0]);
                int y = ParseInt32(split[1]);

                split = area.Split('x');

                int width = ParseInt32(split[0]);
                int height = ParseInt32(split[1]);

                return new Claim()
                {
                    Id = id,
                    X = x,
                    Y = y,
                    Width = width,
                    Height = height,
                };
            }
        }

        /// <summary>
        /// A class representing a square of the fabric. This class cannot be inherited.
        /// </summary>
        private class Square
        {
            /// <summary>
            /// Gets the Ids of the claims associated with the square.
            /// </summary>
            internal List<string> Claims { get; } = new List<string>();
        }
    }
}
