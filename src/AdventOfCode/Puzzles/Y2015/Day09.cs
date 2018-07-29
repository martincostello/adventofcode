// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/9</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day09 : Puzzle2015
    {
        /// <summary>
        /// Gets the solution.
        /// </summary>
        internal int Solution { get; private set; }

        /// <summary>
        /// Gets the shortest distance to visit all of the specified locations
        /// exactly once and starting and ending at distinct separate points.
        /// </summary>
        /// <param name="collection">A collection of distances.</param>
        /// <returns>
        /// The shortest possible distance to visit all the specified locations exactly once.
        /// </returns>
        internal static int GetShortestDistanceBetweenPoints(ICollection<string> collection)
        {
            return GetDistanceBetweenPoints(collection, findLongest: false);
        }

        /// <summary>
        /// Gets the distance to visit all of the specified locations exactly
        /// once and starting and ending at distinct separate points.
        /// </summary>
        /// <param name="collection">A collection of distances.</param>
        /// <param name="findLongest">Whether to find the longest distance.</param>
        /// <returns>
        /// The shortest or longest possible distance to visit all the specified locations exactly once.
        /// </returns>
        internal static int GetDistanceBetweenPoints(ICollection<string> collection, bool findLongest)
        {
            // Parse the input
            var parsedDistances = collection
                .Select((p) => p.Split(new[] { " = " }, StringSplitOptions.None))
                .Select((p) => new { Locations = p[0].Split(new[] { " to " }, StringSplitOptions.None), Distance = ParseInt32(p[1]) })
                .ToList();

            if (parsedDistances.Count == 1)
            {
                // Trivial case
                return parsedDistances[0].Distance;
            }

            // How many unique locations can be started from?
            var uniqueLocations = parsedDistances
                .SelectMany((p) => p.Locations)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            // Get the possible next destination and the distance to it from each unique location
            var possibleDestinations = new Dictionary<string, IDictionary<string, int>>();

            foreach (string location in uniqueLocations)
            {
                var distancesFeaturingLocation = parsedDistances
                    .Where((p) => p.Locations.Contains(location))
                    .ToList();

                var destinations = new Dictionary<string, int>();

                foreach (var distancePair in distancesFeaturingLocation)
                {
                    string other;

                    if (string.Equals(location, distancePair.Locations[0], StringComparison.Ordinal))
                    {
                        other = distancePair.Locations[1];
                    }
                    else
                    {
                        other = distancePair.Locations[0];
                    }

                    destinations[other] = distancePair.Distance;
                }

                possibleDestinations[location] = destinations;
            }

            // Get the distance of each possible path
            var paths = new List<Path>();

            // Walk the paths from all of the possible origin points
            foreach (string origin in uniqueLocations)
            {
                paths.AddRange(WalkPaths(origin, possibleDestinations));
            }

            // Find the length of the longest path
            int maxPathLength = paths.Max((p) => p.Steps.Count);

            // Discount any paths that did not visit all locations
            var completePaths = paths
                .Where((p) => p.Steps.Count == maxPathLength);

            var orderedPaths = completePaths.OrderBy((p) => p.PathDistance);

            var pathOfInterest = findLongest ? orderedPaths.Last() : orderedPaths.First();

            return pathOfInterest.PathDistance;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            bool findLongest =
                args.Length == 1 &&
                string.Equals(args[0], bool.TrueString, StringComparison.OrdinalIgnoreCase);

            Solution = GetDistanceBetweenPoints(ReadResourceAsLines(), findLongest);

            if (Verbose)
            {
                if (findLongest)
                {
                    Console.WriteLine("The distance of the longest route is {0:N0}.", Solution);
                }
                else
                {
                    Console.WriteLine("The distance of the shortest route is {0:N0}.", Solution);
                }
            }

            return 0;
        }

        /// <summary>
        /// Walks all of the possible paths from a specified origin point where each point is visited exactly once.
        /// </summary>
        /// <param name="origin">The origin point.</param>
        /// <param name="pathsFromSources">The possible next paths and the distances to them from each location.</param>
        /// <returns>An <see cref="IList{T}"/> containing all the possible paths that start at <paramref name="origin"/>.</returns>
        private static IList<Path> WalkPaths(string origin, IDictionary<string, IDictionary<string, int>> pathsFromSources)
        {
            Path path = new Path(origin);

            var pathsWalked = new List<Path>()
            {
                path,
            };

            // Walk all the possible paths from the origin point
            foreach (var next in pathsFromSources[origin])
            {
                WalkPaths(next.Key, path, pathsWalked, pathsFromSources);
            }

            return pathsWalked;
        }

        /// <summary>
        /// Walks all of the possible paths from a specified origin point where each point is visited exactly once.
        /// </summary>
        /// <param name="current">The current point.</param>
        /// <param name="currentPath">The path that lead to the current point.</param>
        /// <param name="pathsWalked">A collection of all the paths walked so far.</param>
        /// <param name="pathsFromSources">The possible next paths and the distances to them from each location.</param>
        private static void WalkPaths(
            string current,
            Path currentPath,
            ICollection<Path> pathsWalked,
            IDictionary<string, IDictionary<string, int>> pathsFromSources)
        {
            if (currentPath.HasVisited(current))
            {
                // We've already been here so walk no further down this route
                return;
            }

            // Create a new path that is the scurrent path plus a step to this position
            int distanceFromPreviousToCurrent = pathsFromSources[currentPath.Current][current];

            Path nextPath = currentPath
                .Clone()
                .Visit(current, distanceFromPreviousToCurrent);

            pathsWalked.Add(nextPath);

            // Recursively continue down the path to the next possible destinations
            foreach (var next in pathsFromSources[nextPath.Current])
            {
                WalkPaths(next.Key, nextPath, pathsWalked, pathsFromSources);
            }
        }

        /// <summary>
        /// A class representing a path between some locations. This class cannot be inherited.
        /// </summary>
        private sealed class Path
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Path"/> class.
            /// </summary>
            /// <param name="origin">The origin point.</param>
            internal Path(string origin)
            {
                Origin = origin;
                Steps = new List<string>(new[] { origin });
            }

            /// <summary>
            /// Gets the origin point.
            /// </summary>
            internal string Origin { get; }

            /// <summary>
            /// Gets the current position on the path.
            /// </summary>
            internal string Current => Steps.Last();

            /// <summary>
            /// Gets the steps along the path.
            /// </summary>
            internal IList<string> Steps { get; private set; }

            /// <summary>
            /// Gets the total distance along the path.
            /// </summary>
            internal int PathDistance { get; private set; }

            /// <inheritdoc />
            public override string ToString() => string.Join(" -> ", Steps);

            /// <summary>
            /// Clones this instance.
            /// </summary>
            /// <returns>A new instance of <see cref="Path"/> cloned from the current instance.</returns>
            internal Path Clone()
            {
                return new Path(Origin)
                {
                    Steps = new List<string>(Steps),
                    PathDistance = PathDistance,
                };
            }

            /// <summary>
            /// Visits the specified location.
            /// </summary>
            /// <param name="location">The location to visit.</param>
            /// <param name="distance">The distance to the location.</param>
            /// <returns>The current <see cref="Path"/>.</returns>
            internal Path Visit(string location, int distance)
            {
                Steps.Add(location);
                PathDistance += distance;

                return this;
            }

            /// <summary>
            /// Determines whether the specified location has already been visited.
            /// </summary>
            /// <param name="location">The location to test for.</param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="location"/> has already been visited; otherwise <see langword="false"/>.
            /// </returns>
            internal bool HasVisited(string location) => Steps.Contains(location);
        }
    }
}
