// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 19, "Medicine for Rudolph", RequiresData = true, IsHidden = true)]
public sealed class Day19 : Puzzle
{
    /// <summary>
    /// Gets the solution for calibration.
    /// </summary>
    public int CalibrationSolution { get; private set; }

    /// <summary>
    /// Gets the solution for fabrication.
    /// </summary>
    public int FabricationSolution { get; private set; }

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
        List<string> replacements,
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
                        molecule[(index + source.Length)..];

                    molecules.Add(newMolecule);
                }
            }
        }

        return molecules
            .Order(StringComparer.Ordinal)
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
    internal static int GetMinimumSteps(string molecule, List<string> replacements, ILogger logger, CancellationToken cancellationToken)
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
        List<string> replacements,
        string current,
        int step,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var nextSteps = GetPossibleMolecules(current, replacements, cancellationToken);

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
        IList<string> lines = await ReadResourceAsLinesAsync();

        string molecule = lines[^1];

        var replacements = lines
            .Take(lines.Count - 1)
            .Where((p) => !string.IsNullOrEmpty(p))
            .ToList();

        CalibrationSolution = GetMinimumSteps(molecule, replacements, Logger, cancellationToken);
        FabricationSolution = GetPossibleMolecules(molecule, replacements, cancellationToken).Count;

        if (Verbose)
        {
            Logger.WriteLine($"The target molecule can be made in a minimum of {CalibrationSolution:N0} steps.");
            Logger.WriteLine($"{CalibrationSolution:N0} distinct molecules can be created from {FabricationSolution:N0} possible replacements.");
        }

        return PuzzleResult.Create(CalibrationSolution, FabricationSolution);
    }
}
