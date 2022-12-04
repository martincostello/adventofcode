// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 01, "The Tyranny of the Rocket Equation", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the total fuel requirement for the modules.
    /// </summary>
    public int TotalFuelRequiredForModules { get; private set; }

    /// <summary>
    /// Gets the total fuel requirement for the rocket, including the fuel's own mass.
    /// </summary>
    public int TotalFuelRequiredForRocket { get; private set; }

    /// <summary>
    /// Gets the fuel requirements for the specified mass, optionally including the fuel itself.
    /// </summary>
    /// <param name="mass">The mass to get the fuel requirements for.</param>
    /// <param name="includeFuel">Whether to include the mass of the fuel itself.</param>
    /// <returns>
    /// The fuel-inclusive fuel requirements for the mass specified by <paramref name="mass"/>.
    /// </returns>
    public static int GetFuelRequirementsForMass(int mass, bool includeFuel)
    {
        int fuel = GetRequiredFuel(mass);

        if (includeFuel && fuel > 0)
        {
            fuel += GetFuelRequirementsForMass(fuel, true);
        }

        return fuel;

        static int GetRequiredFuel(int mass)
            => Math.Max((mass / 3) - 2, 0);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> masses = await ReadResourceAsNumbersAsync<int>();

        TotalFuelRequiredForModules = masses.Sum((p) => GetFuelRequirementsForMass(p, includeFuel: false));
        TotalFuelRequiredForRocket = masses.Sum((p) => GetFuelRequirementsForMass(p, includeFuel: true));

        if (Verbose)
        {
            Logger.WriteLine("{0} fuel is required for the modules.", TotalFuelRequiredForModules);
            Logger.WriteLine("{0} fuel is required for the fully-fuelled rocket.", TotalFuelRequiredForRocket);
        }

        return PuzzleResult.Create(TotalFuelRequiredForModules, TotalFuelRequiredForRocket);
    }
}
