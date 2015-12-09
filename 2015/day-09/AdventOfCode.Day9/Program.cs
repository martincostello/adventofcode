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
        /// Gets the shortest distance to visit all of the specified locations once and
        /// starting and ending at distinct separate points.
        /// </summary>
        /// <param name="collection">A collection of distances.</param>
        /// <returns>
        /// The shortest possible distance to visit all the specified locations once.
        /// </returns>
        internal static int GetShortestDistanceBetweenPoints(ICollection<string> collection)
        {
            var parsedDistances = collection
                .Select((p) => p.Split(new[] { " = " }, StringSplitOptions.None))
                .Select((p) => new { Locations = p[0].Split(new[] { " to " }, StringSplitOptions.None), Distance = int.Parse(p[1]) })
                .ToList();

            if (parsedDistances.Count == 1)
            {
                return parsedDistances[0].Distance;
            }

            var uniqueLocations = parsedDistances
                .SelectMany((p) => p.Locations)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            IDictionary<string, IDictionary<string, int>> possibleDestinationsFromSource = new Dictionary<string, IDictionary<string, int>>();

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

                possibleDestinationsFromSource[location] = destinations;
            }

            var pathDistances = new List<int>();

            // TODO Implement finding the path distances

            return pathDistances
                .DefaultIfEmpty()
                .Min();
        }
    }
}
