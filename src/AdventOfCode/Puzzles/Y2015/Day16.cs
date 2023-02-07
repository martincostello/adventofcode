// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 16, "Aunt Sue", RequiresData = true)]
public sealed class Day16 : Puzzle
{
    /// <summary>
    /// The result of the forensic analysis of the gift from Aunt Sue X
    /// as provided by the My First Crime Scene Analysis Machine (MFCSAM).
    /// </summary>
    private static readonly Dictionary<string, (int Count, int Operand)> ForensicAnalysis = new(10)
    {
        ["children"] = (3, 0),
        ["cats"] = (7, 1),
        ["samoyeds"] = (2, 0),
        ["pomeranians"] = (3, -1),
        ["akitas"] = (0, 0),
        ["vizslas"] = (0, 0),
        ["goldfish"] = (5, -1),
        ["trees"] = (3, 1),
        ["cars"] = (2, 0),
        ["perfumes"] = (1, 0),
    };

    /// <summary>
    /// Gets the number of the Aunt Sue that sent the gift.
    /// </summary>
    internal int AuntSueNumber { get; private set; }

    /// <summary>
    /// Gets the number of the real Aunt Sue that sent the gift.
    /// </summary>
    internal int RealAuntSueNumber { get; private set; }

    /// <summary>
    /// Returns the number of the Aunt Sue that sent the gift from the specified Aunt Sue metadata.
    /// </summary>
    /// <param name="metadata">The metadata about all the Aunt Sues.</param>
    /// <param name="compensateForRetroEncabulator">Whether to compensate for the Retro Encabulator.</param>
    /// <returns>
    /// The number of the Aunt Sue which sent the gift based on forensic analysis of <paramref name="metadata"/>.
    /// </returns>
    internal static int WhichAuntSueSentTheGift(ICollection<string> metadata, bool compensateForRetroEncabulator = false)
    {
        var parsed = metadata.Select(AuntSue.Parse);

        foreach (var item in ForensicAnalysis)
        {
            if (compensateForRetroEncabulator && item.Value.Operand != 0)
            {
                if (item.Value.Operand == 1)
                {
                    bool NotFoundOrMore(AuntSue p)
                    {
                        if (!p.Metadata.TryGetValue(item.Key, out int value))
                        {
                            return true;
                        }

                        return value > item.Value.Count;
                    }

                    parsed = parsed.Where((p) => NotFoundOrMore(p));
                }
                else
                {
                    bool NotFoundOrLess(AuntSue p)
                    {
                        if (!p.Metadata.TryGetValue(item.Key, out int value))
                        {
                            return true;
                        }

                        return value < item.Value.Count;
                    }

                    parsed = parsed.Where((p) => NotFoundOrLess(p));
                }
            }
            else
            {
                bool NotFoundOrEqual(AuntSue p)
                {
                    if (!p.Metadata.TryGetValue(item.Key, out int value))
                    {
                        return true;
                    }

                    return value == item.Value.Count;
                }

                parsed = parsed.Where((p) => NotFoundOrEqual(p));
            }
        }

        return parsed.Single().Number;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> auntSueMetadata = await ReadResourceAsLinesAsync(cancellationToken);

        AuntSueNumber = WhichAuntSueSentTheGift(auntSueMetadata);
        RealAuntSueNumber = WhichAuntSueSentTheGift(auntSueMetadata, compensateForRetroEncabulator: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The number of the Aunt Sue that got me the gift was originally thought to be {0}, but it was actually {1}.",
                AuntSueNumber,
                RealAuntSueNumber);
        }

        return PuzzleResult.Create(AuntSueNumber, RealAuntSueNumber);
    }

    /// <summary>
    /// A class representing an aunt called Sue. This class cannot be inherited.
    /// </summary>
    private sealed class AuntSue
    {
        /// <summary>
        /// Gets the number of the Aunt Sue.
        /// </summary>
        internal int Number { get; private set; }

        /// <summary>
        /// Gets the metadata about this Aunt Sue.
        /// </summary>
        internal Dictionary<string, int> Metadata { get; } = new();

        /// <summary>
        /// Parses an instance of <see cref="AuntSue"/> from the specified <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <returns>
        /// An instance of <see cref="AuntSue"/> that represents <paramref name="value"/>.
        /// </returns>
        internal static AuntSue Parse(string value)
        {
            var result = new AuntSue();

            string[] split = value.Split(' ');

            result.Number = Parse<int>(split[1].TrimEnd(':'));

            foreach (var item in string.Join(' ', split, 2, split.Length - 2).Tokenize(','))
            {
                (string first, string second) = item.Bifurcate(':');
                result.Metadata[first.Trim()] = Parse<int>(second.Trim());
            }

            return result;
        }
    }
}
