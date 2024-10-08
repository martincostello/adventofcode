﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 04, "Passport Processing", RequiresData = true)]
public sealed partial class Day04 : Puzzle
{
    /// <summary>
    /// The required keys for passports. This field is read-only.
    /// </summary>
    private static readonly ImmutableArray<string> RequiredKeys = ["byr", "ecl", "eyr", "hcl", "hgt", "iyr", "pid"];

    /// <summary>
    /// Gets the number of valid passports.
    /// </summary>
    public int ValidPassports { get; private set; }

    /// <summary>
    /// Gets the number of verified passports.
    /// </summary>
    public int VerifiedPassports { get; private set; }

    [GeneratedRegex("#[0-9a-f]{6}", RegexOptions.IgnoreCase)]
    private static partial Regex HexColor { get; }

    /// <summary>
    /// Gets the number of valid passports in the specified batch file.
    /// </summary>
    /// <param name="batch">The batch file to get the number of valid passports from.</param>
    /// <returns>
    /// The number of valid passports specified by <paramref name="batch"/>.
    /// </returns>
    public static (int Valid, int Verified) VerifyPassports(ICollection<string> batch)
    {
        var passports = new List<Dictionary<string, string>>();
        var current = new Dictionary<string, string>();

        foreach (string item in batch)
        {
            var line = item.AsSpan();

            if (line.IsEmpty)
            {
                passports.Add(current);
                current = [];
                continue;
            }

            foreach (var range in line.Split(' '))
            {
                line[range].Bifurcate(':', out var name, out var value);
                current[new string(name)] = new(value);
            }
        }

        if (current.Count > 0)
        {
            passports.Add(current);
        }

        int valid = 0;
        int verified = 0;

        foreach (var passport in passports)
        {
            bool isValid = passport.Keys.Intersect(RequiredKeys).ExactCount(RequiredKeys.Length);

            if (isValid)
            {
                valid++;

                if (IsPasswordVerified(passport))
                {
                    verified++;
                }
            }
        }

        return (valid, verified);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var batch = await ReadResourceAsLinesAsync(cancellationToken);

        (ValidPassports, VerifiedPassports) = VerifyPassports(batch);

        if (Verbose)
        {
            Logger.WriteLine("There are {0} valid passports.", ValidPassports);
            Logger.WriteLine("There are {0} verified passports.", VerifiedPassports);
        }

        return PuzzleResult.Create(ValidPassports, VerifiedPassports);
    }

    /// <summary>
    /// Returns whether the specified passport is verified.
    /// </summary>
    /// <param name="passport">The passport to verify.</param>
    /// <returns>
    /// <see langword="true"/> if the passport is verified; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsPasswordVerified(Dictionary<string, string> passport)
    {
        static bool IsValidEyeColor(ReadOnlySpan<char> value) => value switch
        {
            "amb" => true,
            "blu" => true,
            "brn" => true,
            "grn" => true,
            "gry" => true,
            "hzl" => true,
            "oth" => true,
            _ => false,
        };

        static bool IsValidHairColor(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty)
            {
                return false;
            }

            return HexColor.IsMatch(value);
        }

        static bool IsValidHeight(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty)
            {
                return false;
            }

            if (value.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                return IsValidNumber(value[..^2], 150, 193);
            }
            else if (value.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                return IsValidNumber(value[..^2], 59, 76);
            }

            return false;
        }

        static bool IsValidNumber(ReadOnlySpan<char> value, int minimum, int maximum)
        {
            if (!TryParse(value, out int number))
            {
                return false;
            }

            return number >= minimum && number <= maximum;
        }

        static bool IsValidYear(ReadOnlySpan<char> value, int minimum, int maximum)
            => IsValidNumber(value, minimum, maximum) && value!.Length == 4;

        foreach (string? key in passport.Keys)
        {
            ReadOnlySpan<char> value = passport[key];

            bool isValid = key switch
            {
                "byr" => IsValidYear(value, 1920, 2002),
                "ecl" => IsValidEyeColor(value),
                "eyr" => IsValidYear(value, 2020, 2030),
                "hcl" => IsValidHairColor(value),
                "hgt" => IsValidHeight(value),
                "iyr" => IsValidYear(value, 2010, 2020),
                "pid" => TryParse(value, out int _) && value!.Length == 9,
                _ => true,
            };

            if (!isValid)
            {
                return false;
            }
        }

        return true;
    }
}
