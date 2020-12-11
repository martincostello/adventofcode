// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/19</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 19, MinimumArguments = 1, RequiresData = true, IsHidden = true)]
    public sealed class Day19 : Puzzle
    {
        /// <summary>
        /// Gets the solution for fabrication or calibration.
        /// </summary>
        internal int Solution { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Gets the possible molecules that can be created from single step transformations of a molecule.
        /// </summary>
        /// <param name="molecule">The input molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The distinct molecules that can be created from <paramref name="molecule"/> using all of the
        /// possible replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static ICollection<string> GetPossibleMolecules(
            string molecule,
            ICollection<string> replacements,
            CancellationToken cancellationToken)
        {
            var molecules = new HashSet<string>();

            foreach (string replacement in replacements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string[] split = replacement.Split(" => ");

                string source = split[0];
                string target = split[1];

                for (int i = 0; i < molecule.Length; i++)
                {
                    int index = molecule.IndexOf(source, i, StringComparison.Ordinal);

                    if (index > -1)
                    {
                        string newMolecule =
                            (index == 0 ? string.Empty : molecule[0..index]) +
                            target +
                            molecule[(index + source.Length) ..];

                        if (!molecules.Contains(newMolecule))
                        {
                            molecules.Add(newMolecule);
                        }
                    }
                }
            }

            return molecules
                .OrderBy((p) => p, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Gets the minimum number of steps that can be performed to make the specified molecule using the specified replacements.
        /// </summary>
        /// <param name="molecule">The desired molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The minimum number of steps required to create <paramref name="molecule"/> using the possible
        /// replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static int GetMinimumSteps(string molecule, ICollection<string> replacements, ILogger logger, CancellationToken cancellationToken)
            => GetMinimumSteps(molecule, replacements, "e", 1, logger, cancellationToken);

        /// <summary>
        /// Gets the minimum number of steps that can be performed to make the specified molecule using the specified replacements.
        /// </summary>
        /// <param name="molecule">The desired molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <param name="current">The current molecule being worked with.</param>
        /// <param name="step">The current step number.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The minimum number of steps required to create <paramref name="molecule"/> using the possible
        /// replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static int GetMinimumSteps(
            string molecule,
            ICollection<string> replacements,
            string current,
            int step,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            ICollection<string> nextSteps = GetPossibleMolecules(current, replacements, cancellationToken);

            if (nextSteps.Contains(molecule))
            {
                logger.WriteLine($"Found solution that takes {step:N0} steps.");
                return step;
            }

            var steps = new List<int>(nextSteps.Count);

            foreach (string next in nextSteps.Where((p) => p.Length < molecule.Length))
            {
                steps.Add(GetMinimumSteps(molecule, replacements, next, step + 1, logger, cancellationToken));
            }

            return steps
                .Where((p) => p > 0)
                .DefaultIfEmpty()
                .Min();
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            bool fabricate = args[0].ToUpperInvariant() switch
            {
                "CALIBRATE" => false,
                "FABRICATE" => true,
                _ => throw new PuzzleException("The mode specified is invalid."),
            };

            IList<string> lines = await ReadResourceAsLinesAsync();

            string molecule = lines[^1];

            ICollection<string> replacements = lines
                .Take(lines.Count - 1)
                .Where((p) => !string.IsNullOrEmpty(p))
                .ToList();

            if (fabricate)
            {
                Solution = GetMinimumSteps(molecule, replacements, Logger, cancellationToken);

                if (Verbose)
                {
                    Logger.WriteLine($"The target molecule can be made in a minimum of {Solution:N0} steps.");
                }
            }
            else
            {
                ICollection<string> molecules = GetPossibleMolecules(molecule, replacements, cancellationToken);

                Solution = molecules.Count;

                if (Verbose)
                {
                    Logger.WriteLine($"{Solution:N0} distinct molecules can be created from {replacements.Count:N0} possible replacements.");
                }
            }

            return PuzzleResult.Create(Solution);
        }
    }
}
