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
    /// The number of distinct molecules that can be created from <paramref name="molecule"/> using all of the
    /// possible replacements specified by <paramref name="replacements"/>.
    /// </returns>
    internal static int GetPossibleMolecules(
        string molecule,
        List<(string Source, string Target)> replacements,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        var molecules = new HashSet<string>();

        foreach ((string source, string target) in replacements)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int i = 0; i < molecule.Length; i++)
            {
                int index = molecule.IndexOf(source, i, StringComparison.Ordinal);

                if (index is -1)
                {
                    break;
                }

                builder.Clear();

                if (index > 0)
                {
                    builder.Append(molecule[..index]);
                }

                builder.Append(target);
                builder.Append(molecule[(index + source.Length)..]);

                molecules.Add(builder.ToString());
            }
        }

        return molecules.Count;
    }

    /// <summary>
    /// Gets the minimum number of steps that can be performed to make the specified molecule using the specified replacements.
    /// </summary>
    /// <param name="molecule">The desired molecule.</param>
    /// <param name="replacements">The possible replacements.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The minimum number of steps required to create <paramref name="molecule"/> using the possible
    /// replacements specified by <paramref name="replacements"/>.
    /// </returns>
    internal static int GetMinimumSteps(
        string molecule,
        List<(string Source, string Target)> replacements,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        int minimum = int.MaxValue;
        int step = 1;

        return Synthesize(molecule, "e", replacements, step, ref minimum, builder, cancellationToken) ? minimum : -1;

        static bool Synthesize(
            string molecule,
            string desired,
            List<(string Source, string Target)> replacements,
            int step,
            ref int minimum,
            StringBuilder builder,
            CancellationToken cancellationToken)
        {
            if (step > minimum)
            {
                return false;
            }

            foreach ((string source, string target) in replacements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int index = molecule.LastIndexOf(target, StringComparison.Ordinal);

                if (index > -1)
                {
                    builder.Clear();

                    if (index > 0)
                    {
                        builder.Append(molecule[..index]);
                    }

                    builder.Append(source);
                    builder.Append(molecule[(index + target.Length)..]);

                    string reduced = builder.ToString();

                    if (reduced == desired)
                    {
                        minimum = step;
                        return true;
                    }
                    else if (Synthesize(reduced, desired, replacements, step + 1, ref minimum, builder, cancellationToken))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var lines = await ReadResourceAsLinesAsync(cancellationToken);

        string molecule = lines[^1];

        var replacements = lines
            .Take(lines.Count - 1)
            .Where((p) => !string.IsNullOrEmpty(p))
            .Select((p) => p.Split(" => "))
            .Select((p) => (p[0], p[1]))
            .OrderByDescending((p) => p.Item2.Length)
            .ToList();

        FabricationSolution = GetPossibleMolecules(molecule, replacements, cancellationToken);
        CalibrationSolution = GetMinimumSteps(molecule, replacements, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine($"{CalibrationSolution:N0} distinct molecules can be created from {FabricationSolution:N0} possible replacements.");
            Logger.WriteLine($"The target molecule can be made in a minimum of {CalibrationSolution:N0} steps.");
        }

        return PuzzleResult.Create(CalibrationSolution, FabricationSolution);
    }
}
