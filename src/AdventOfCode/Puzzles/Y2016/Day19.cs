// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/19</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 19, MinimumArguments = 1)]
    public sealed class Day19 : Puzzle
    {
        /// <summary>
        /// Gets the number of the elf who receives all the presents.
        /// </summary>
        public int ElfWithAllPresents { get; private set; }

        /// <summary>
        /// Finds the elf that receives all of the presents.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <param name="version">The version of the rules to use.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        internal static int FindElfThatGetsAllPresents(int count, int version)
        {
            return
                version == 2 ?
                FindElfThatGetsAllPresentsV2(count) :
                FindElfThatGetsAllPresentsV1(count);
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            int count = ParseInt32(args[0]);
            int version = args.Length > 1 ? ParseInt32(args[1]) : 1;

            ElfWithAllPresents = FindElfThatGetsAllPresents(count, version);

            if (Verbose)
            {
                Logger.WriteLine($"The elf that gets all the presents using version {version} of the rules is {ElfWithAllPresents:N0}.");
            }

            return PuzzleResult.Create(ElfWithAllPresents);
        }

        /// <summary>
        /// Finds the elf that receives all of the presents using version 1 of the rules.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        private static int FindElfThatGetsAllPresentsV1(int count)
        {
            var circle = new LinkedList<int>(Enumerable.Range(1, count));
            var current = circle.First!;

            while (circle.Count > 1)
            {
                circle.Remove(circle.Clockwise(current));
                current = circle.Clockwise(current);
            }

            return circle.First!.Value;
        }

        /// <summary>
        /// Finds the elf that receives all of the presents using version 2 of the rules.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        private static int FindElfThatGetsAllPresentsV2(int count)
        {
            var circle = new LinkedList<int>(Enumerable.Range(1, count));
            var current = circle.First!;
            var opposite = current;

            int steps = circle.Count / 2;

            for (int i = 0; i < steps; i++)
            {
                opposite = circle.Clockwise(opposite);
            }

            while (circle.Count > 1)
            {
                var next = circle.Clockwise(opposite);

                if (circle.Count % 2 == 1)
                {
                    next = circle.Clockwise(next);
                }

                circle.Remove(opposite);

                opposite = next;

                current = circle.Clockwise(current);
            }

            return circle.First!.Value;
        }
    }
}
