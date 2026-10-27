// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 13, "Knights of the Dinner Table", RequiresData = true)]
public sealed class Day13 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the maximum total change in happiness for the specified potential happiness of the guests.
    /// </summary>
    /// <param name="potentialHappiness">A collection of potential guess happinesses.</param>
    /// <returns>The optional total change in happiness for the specified potentials.</returns>
    internal static int GetMaximumTotalChangeInHappiness(ICollection<string> potentialHappiness)
    {
        // Parse the input data
        var potentials = potentialHappiness
            .Select(ParsePotentialHappiness)
            .ToList();

        // Determine all of the guests
        var names = potentials
            .Select((p) => p.Name)
            .Distinct()
            .ToList();

        // Key the happiness for each person for the people they could sit next to
        int[,] happinesses = new int[names.Count, names.Count];

        foreach (var potential in potentials)
        {
            happinesses[names.IndexOf(potential.Name), names.IndexOf(potential.AdjacentName)] = potential.Happiness;
        }

        // Get the maximum potential happiness from all the seating arrangements
        int[] setting = new int[names.Count];

        for (int i = 0; i < setting.Length; i++)
        {
            setting[i] = i;
        }

        int maximum = int.MinValue;

        Permute(setting, 0, happinesses, ref maximum);

        return maximum;

        static void Permute(int[] setting, int index, int[,] happinesses, ref int maximum)
        {
            if (index == setting.Length)
            {
                maximum = Math.Max(maximum, GetHappiness(setting, happinesses));
                return;
            }

            for (int i = index; i < setting.Length; i++)
            {
                (setting[index], setting[i]) = (setting[i], setting[index]);

                Permute(setting, index + 1, happinesses, ref maximum);

                (setting[index], setting[i]) = (setting[i], setting[index]);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (potentialHappiness, logger, _) =>
            {
                int maximumTotalChangeInHappiness = GetMaximumTotalChangeInHappiness(potentialHappiness);

                // Create a new guest list which is the same as the previous one but with the current user added
                var potentialHappinessWithCurrentUser = new List<string>(potentialHappiness);

                var existingGuestNames = potentialHappiness
                    .Select((p) => p.Split(' ')[0])
                    .Distinct()
                    .ToList();

                // Add the current user to the guest list where everyone is neutral to them
                string currentUser = "You";

                const string AmbivalentFormat = "{0} would gain 0 happiness units by sitting next to {1}.";

                foreach (string guest in existingGuestNames)
                {
                    potentialHappinessWithCurrentUser.Add(Format(AmbivalentFormat, guest, currentUser));
                    potentialHappinessWithCurrentUser.Add(Format(AmbivalentFormat, currentUser, guest));
                }

                int maximumTotalChangeInHappinessWithCurrentUser = GetMaximumTotalChangeInHappiness(potentialHappinessWithCurrentUser);

                if (logger is { })
                {
                    logger.WriteLine("The total change in happiness is {0:N0}.", maximumTotalChangeInHappiness);
                    logger.WriteLine("The total change in happiness with the current user seated is {0:N0}.", maximumTotalChangeInHappinessWithCurrentUser);
                }

                return (maximumTotalChangeInHappiness, maximumTotalChangeInHappinessWithCurrentUser);
            },
            cancellationToken);
    }

    /// <summary>
    /// Gets the total happiness for the specified place setting.
    /// </summary>
    /// <param name="setting">The place setting.</param>
    /// <param name="happinesses">The potential happinesses.</param>
    /// <returns>The total change in happiness for the given place setting.</returns>
    private static int GetHappiness(ReadOnlySpan<int> setting, int[,] happinesses)
    {
        int head = setting[0];
        int tail = setting[^1];

        // Because the table is circular, find the happinesses for the first and last items
        int happiness = happinesses[head, tail];
        happiness += happinesses[tail, head];
        happiness += happinesses[head, setting[1]];
        happiness += happinesses[tail, setting[^2]];

        // Find the happiness for each seat which has a seat on either side
        for (int i = 1; i < setting.Length - 1; i++)
        {
            int current = setting[i];
            happiness += happinesses[current, setting[i - 1]];
            happiness += happinesses[current, setting[i + 1]];
        }

        return happiness;
    }

    /// <summary>
    /// Parses the potential happiness from the specified value.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>The <see cref="Potential"/> representation of <paramref name="value"/>.</returns>
    private static Potential ParsePotentialHappiness(string value)
    {
        string[] split = value.Split(' ');

        var result = new Potential()
        {
            Name = split[0],
            AdjacentName = split[^1].TrimEnd('.'),
            Happiness = Parse<int>(split[3]),
        };

        if (split[2] == "lose")
        {
            result.Happiness *= -1;
        }

        return result;
    }

    /// <summary>
    /// A class representing a happiness potential for a person sat next to another person. This class cannot be inherited.
    /// </summary>
    private sealed class Potential
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        internal string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the adjacent person.
        /// </summary>
        internal string AdjacentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the potential happiness.
        /// </summary>
        internal int Happiness { get; set; }
    }
}
