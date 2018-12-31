// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2018/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : Puzzle2018
    {
        /// <summary>
        /// Gets the area, in square inches, of fabric with two or more claims.
        /// </summary>
        public int Area { get; private set; }

        /// <summary>
        /// Calculates the number of square inches of fabric with two or more overlapping claims.
        /// </summary>
        /// <param name="claims">The claimed areas of the fabric.</param>
        /// <returns>
        /// The area, in square inches, of fabric with two or more claims specified by <paramref name="claims"/>.
        /// </returns>
        public static int GetAreaWithTwoOrMoreOverlappingClaims(IEnumerable<string> claims)
        {
            IList<Claim> fabricClaims = claims
                .Select(Claim.Parse)
                .ToList();

            int totalWidth = fabricClaims.Max((p) => p.X + p.Width);
            int totalHeight = fabricClaims.Max((p) => p.Y + p.Height);

            var fabric = new int[totalWidth, totalHeight];

            foreach (var claim in fabricClaims)
            {
                for (int i = claim.X; i < claim.Width + claim.X; i++)
                {
                    for (int j = claim.Y; j < claim.Height + claim.Y; j++)
                    {
                        fabric[i, j]++;
                    }
                }
            }

            int result = 0;

            for (int i = 0; i < totalWidth; i++)
            {
                for (int j = 0; j < totalHeight; j++)
                {
                    if (fabric[i, j] >= 2)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> claims = ReadResourceAsLines();

            Area = GetAreaWithTwoOrMoreOverlappingClaims(claims);

            if (Verbose)
            {
                Logger.WriteLine($"{Area:N0} square inches of fabric are within two or more claims.");
            }

            return 0;
        }

        /// <summary>
        /// A class representing a claim for fabric. This class cannot be inherited.
        /// </summary>
        private sealed class Claim
        {
            /// <summary>
            /// Gets the Id of the claim.
            /// </summary>
            internal string Id { get; private set; }

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
                string[] split = claim.Split(Arrays.Space);

                string id = split[0];
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
    }
}
