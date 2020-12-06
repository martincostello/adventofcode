// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/5</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2019, 05, MinimumArguments = 1, RequiresData = true)]
    public sealed class Day05 : Puzzle
    {
        /// <summary>
        /// Gets the diagnostic code output by the program.
        /// </summary>
        public long DiagnosticCode { get; private set; }

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The diagnostic code output by the program.
        /// </returns>
        public static async Task<long> RunProgramAsync(
            string program,
            long input,
            CancellationToken cancellationToken)
        {
            long[] instructions = IntcodeVM.ParseProgram(program);

            var vm = new IntcodeVM(instructions)
            {
                Input = await ChannelHelpers.CreateReaderAsync(new[] { input }, cancellationToken),
            };

            if (!await vm.RunAsync(cancellationToken))
            {
                throw new PuzzleException("Failed to run program.");
            }

            var outputs = await vm.Output.ToListAsync(cancellationToken);
            return outputs.Count == 0 ? 0 : outputs[^1];
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            int input = ParseInt32(args[0]);
            string program = await ReadResourceAsStringAsync();

            DiagnosticCode = await RunProgramAsync(program, input, cancellationToken);

            if (Verbose)
            {
                Logger.WriteLine("The program produces diagnostic code {0}.", DiagnosticCode);
            }

            return PuzzleResult.Create(DiagnosticCode);
        }
    }
}
