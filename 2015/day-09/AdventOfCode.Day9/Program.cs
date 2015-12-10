// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day9
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/9</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            int shortestDistance = GetShortestDistanceBetweenPoints(File.ReadAllLines(args[0]));

            Console.WriteLine("The distance of the shortest route is {0:N0}.", shortestDistance);

            return 0;
        }

        /// <summary>
        /// Gets the shortest distance to visit all of the specified locations
        /// exactly once and starting and ending at distinct separate points.
        /// </summary>
        /// <param name="collection">A collection of distances.</param>
        /// <returns>
        /// The shortest possible distance to visit all the specified locations once.
        /// </returns>
        internal static int GetShortestDistanceBetweenPoints(ICollection<string> collection)
        {
            // Parse the input
            var parsedDistances = collection
                .Select((p) => p.Split(new[] { " = " }, StringSplitOptions.None))
                .Select((p) => new { Locations = p[0].Split(new[] { " to " }, StringSplitOptions.None), Distance = int.Parse(p[1]) })
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
            IDictionary<string, IDictionary<string, int>> possibleDestinations = new Dictionary<string, IDictionary<string, int>>();

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
                .Where((p) => p.Steps.Count == maxPathLength)
                .OrderBy((p) => p.ToString())
                .ToList();

            var shortestPath = completePaths
                .OrderBy((p) => p.PathDistance)
                .First();

            System.Diagnostics.Trace.WriteLine(shortestPath);

            // Find the shortest complete path
            return shortestPath.PathDistance;
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
        /// <returns>An <see cref="IList{T}"/> containing all the possible paths that start at <paramref name="origin"/>.</returns>
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

            Path nextPath = currentPath.Clone();
            nextPath.Visit(current, distanceFromPreviousToCurrent);

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
            internal void Visit(string location, int distance)
            {
                Steps.Add(location);
                PathDistance += distance;
            }

            /// <summary>
            /// Determines whether the specified location has already been visited.
            /// </summary>
            /// <param name="location">The location to test for.</param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="location"/> has already been visited; otherwise <see langword="false"/>.
            /// </returns>
            internal bool HasVisited(string location) => Steps.Contains(location);

            /// <inheritdoc />
            public override string ToString() => string.Join(" -> ", Steps);
        }
    }
}
