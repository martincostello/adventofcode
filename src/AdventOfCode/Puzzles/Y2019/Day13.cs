// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/13</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day13 : Puzzle2019
    {
        /// <summary>
        /// Gets the number of block tiles on the screen.
        /// </summary>
        public long BlockTileCount { get; private set; }

        /// <summary>
        /// Gets the number of block tiles on the screen after the game is run.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <returns>
        /// The number of block tiles on the screen.
        /// </returns>
        public static async Task<long> GetBlockTileCountAsync(string program)
        {
            long[] instructions = IntcodeVM.ParseProgram(program);

            var vm = new IntcodeVM(instructions, 10_000);

            if (!await vm.RunAsync())
            {
                throw new InvalidProgramException();
            }

            var outputs = await vm.Output.ToListAsync();

            int blockTiles = 0;

            for (int i = 0; i < outputs.Count; i += 3)
            {
                long tileId = outputs[i + 2];

                if (tileId == 2)
                {
                    blockTiles++;
                }
            }

            return blockTiles;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            BlockTileCount = GetBlockTileCountAsync(program).Result;

            if (Verbose)
            {
                Logger.WriteLine("There are {0} block tiles on the screen when the game exits.", BlockTileCount);
            }

            return 0;
        }
    }
}
