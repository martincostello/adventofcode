// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

#pragma warning disable SA1008

using System.Runtime.CompilerServices;
using Lens = (int Sticker, int FocalLength);

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 15, "Lens Library", RequiresData = true)]
public sealed class Day15 : Puzzle
{
    private const int Boxes = 256;

    /// <summary>
    /// Gets the sum of the hash values of the initialization sequence.
    /// </summary>
    public int HashSum { get; private set; }

    /// <summary>
    /// Gets the focusing power of the lens configuration.
    /// </summary>
    public int FocusingPower { get; private set; }

    /// <summary>
    /// Computes the hash of the specified initialization sequence.
    /// </summary>
    /// <param name="sequence">The sequence to hash.</param>
    /// <returns>
    /// The sum of the hash values of <paramref name="sequence"/>.
    /// </returns>
    public static int HashSequence(ReadOnlySpan<char> sequence)
    {
        int sum = 0;
        int next;

        while ((next = sequence.IndexOf(',')) != -1)
        {
            sum += Hash(sequence[..next]).Box;
            sequence = sequence[(next + 1)..];
        }

        sum += Hash(sequence).Box;

        return sum;
    }

    /// <summary>
    /// Initializes the lenses using the specified initialization sequence.
    /// </summary>
    /// <param name="sequence">The initialization sequence.</param>
    /// <returns>
    /// The focusing power of the lens configuration.
    /// </returns>
    public static int Initialize(ReadOnlySpan<char> sequence)
    {
        var boxes = new List<Lens>[Boxes];

        int power = 0;
        int next;

        while ((next = sequence.IndexOf(',')) != -1)
        {
            Shuffle(sequence, next, boxes);
            sequence = sequence[(next + 1)..];
        }

        Shuffle(sequence, sequence.Length, boxes);

        for (int j = 0; j < boxes.Length; j++)
        {
            if (boxes[j] is not { } lenses)
            {
                continue;
            }

            int number = j + 1;
            int slots = lenses.Count;

            for (int i = 0; i < slots; i++)
            {
                (_, int focalLength) = lenses[i];
                power += number * (i + 1) * focalLength;
            }
        }

        return power;

        static void Shuffle(ReadOnlySpan<char> sequence, int next, List<Lens>[] boxes)
        {
            var step = sequence[..next];
            int index = step.IndexOfAnyExceptInRange('a', 'z');

            var label = step[..index];
            (int box, int sticker) = Hash(label);

            var lenses = boxes[box];

            if (lenses is null)
            {
                boxes[box] = lenses = [];
            }

            var operation = step[index..];
            char symbol = operation[0];

            index = lenses.FindIndex((p) => p.Sticker == sticker);

            if (index is -1)
            {
                index = lenses.Count;
            }
            else
            {
                lenses.RemoveAt(index);
            }

            if (symbol is '=')
            {
                int focalLength = Parse<int>(operation[1..]);
                var lens = (sticker, focalLength);

                lenses.Insert(index, lens);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string initializationSequence = await ReadResourceAsStringAsync(cancellationToken);
        initializationSequence = initializationSequence.ReplaceLineEndings(string.Empty);

        HashSum = HashSequence(initializationSequence);
        FocusingPower = Initialize(initializationSequence);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the hash values of the initialization sequence is {0}.", HashSum);
            Logger.WriteLine("The focusing power of the lens configuration is {0}.", FocusingPower);
        }

        return PuzzleResult.Create(HashSum, FocusingPower);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (int Box, int Sticker) Hash(ReadOnlySpan<char> value)
    {
        int box = 0;
        int sticker = 0;

        for (int i = 0; i < value.Length; i++)
        {
            char ch = value[i];

            box += ch;
            box *= 17;
            box %= Boxes;

            sticker = HashCode.Combine(sticker, ch);
        }

        return (box, sticker);
    }
}
