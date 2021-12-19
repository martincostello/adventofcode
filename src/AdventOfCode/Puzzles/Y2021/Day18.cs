// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 18, "Snailfish", RequiresData = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the magnitude of the sum of the snail numbers.
    /// </summary>
    public int MagnitudeOfSum { get; private set; }

    /// <summary>
    /// Gets the largest magnitude of the sum of any two of the snail numbers.
    /// </summary>
    public int LargestSumMagnitude { get; private set; }

    /// <summary>
    /// Calculates the sum of the specified snail numbers.
    /// </summary>
    /// <param name="numbers">The snail numbers to sum.</param>
    /// <returns>
    /// The magnitude of the sum of the snail numbers and the
    /// largest magnitude of any two of the snail numbers.
    /// </returns>
    public static (int MagnitudeOfSum, int LargestSumMagnitude) Sum(IList<string> numbers)
    {
        List<SnailPair> pairs = ParseRaw(numbers);

        SnailPair sum = pairs.Aggregate((x, y) => (x + y).Reduce());

        var magnitudes = new List<int>();

        for (int i = 0; i < numbers.Count - 1; i++)
        {
            for (int j = 0; j < numbers.Count; j++)
            {
                SnailPair x = ParseRaw(new[] { numbers[i] })[0];
                SnailPair y = ParseRaw(new[] { numbers[j] })[0];

                SnailPair z1 = (x.Clone() + y.Clone()).Reduce();
                SnailPair z2 = (y.Clone() + x.Clone()).Reduce();

                magnitudes.Add(z1.Magnitude());
                magnitudes.Add(z2.Magnitude());
            }
        }

        return (sum.Magnitude(), magnitudes.Max());
    }

    /// <summary>
    /// Gets the magnitude of the specified snail number.
    /// </summary>
    /// <param name="number">The snail number to get the magnitude of.</param>
    /// <returns>
    /// The magnitude of the snail number.
    /// </returns>
    internal static int Magnitude(string number)
    {
        SnailPair pair = ParseRaw(new[] { number })[0];
        return pair.Magnitude();
    }

    /// <summary>
    /// Parses the specified snail number.
    /// </summary>
    /// <param name="number">The snail number to parse.</param>
    /// <returns>
    /// The parsed snail number.
    /// </returns>
    internal static string Parse(string number)
    {
        SnailPair pair = ParseRaw(new[] { number })[0];
        return pair.ToString();
    }

    /// <summary>
    /// Reduces the specified snail number.
    /// </summary>
    /// <param name="number">The snail number to reduce.</param>
    /// <returns>
    /// The reduced snail number.
    /// </returns>
    internal static string Reduce(string number)
    {
        SnailPair pair = ParseRaw(new[] { number })[0];
        pair = pair.Reduce();
        return pair.ToString();
    }

    /// <summary>
    /// Calculates the sum of the specified snail numbers.
    /// </summary>
    /// <param name="numbers">The snail numbers to sum.</param>
    /// <returns>
    /// The sum of the two snail numbers.
    /// </returns>
    internal static string SumValues(params string[] numbers)
    {
        List<SnailPair> pairs = ParseRaw(numbers);

        SnailPair sum = pairs.Aggregate((x, y) => (x + y).Reduce());

        return sum.ToString();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> numbers = await ReadResourceAsLinesAsync();

        (MagnitudeOfSum, LargestSumMagnitude) = Sum(numbers);

        if (Verbose)
        {
            Logger.WriteLine("The magnitude of the final sum is {0:N0}.", MagnitudeOfSum);
            Logger.WriteLine("The largest magnitude of any sum of two numbers is {0:N0}.", LargestSumMagnitude);
        }

        return PuzzleResult.Create(MagnitudeOfSum, LargestSumMagnitude);
    }

    private static List<SnailPair> ParseRaw(IList<string> numbers)
    {
        var pairs = new List<SnailPair>(numbers.Count);

        foreach (string number in numbers)
        {
            // Trim off the square brackets of the outer-most pair
            var slice = number.AsSpan()[1..^1];
            pairs.Add(Parse(slice, out _));
        }

        return pairs;

        static SnailPair Parse(ReadOnlySpan<char> number, out int consumed)
        {
            consumed = 0;

            var pair = new SnailPair();

            for (int i = 0; i < number.Length; i++)
            {
                char ch = number[i];
                consumed++;

                if (ch == '[')
                {
                    SnailPair child = Parse(number[consumed..], out int childConsumed);
                    child.Parent = pair;

                    consumed += childConsumed;
                    i += childConsumed;

                    if (pair.Left is null)
                    {
                        pair.Left = child;
                    }
                    else
                    {
                        pair.Right = child;
                        break;
                    }
                }
                else if (ch == ']' || ch == ',')
                {
                    continue;
                }
                else
                {
                    var value = new SnailValue(ch - '0', pair);

                    if (pair.Left is null)
                    {
                        pair.Left = value;
                    }
                    else
                    {
                        pair.Right = value;
                        break;
                    }
                }
            }

            return pair;
        }
    }

    private abstract class SnailNumber
    {
        public SnailPair? Parent { get; set; }

        public static SnailPair operator +(SnailNumber a, SnailNumber b)
        {
            var result = new SnailPair()
            {
                Left = a,
                Right = b,
            };

            result.Left.Parent = result;
            result.Right.Parent = result;

            return result;
        }

        public abstract int Magnitude();
    }

    private class SnailPair : SnailNumber
    {
        public SnailNumber Left { get; set; } = default!;

        public SnailNumber Right { get; set; } = default!;

        public override string ToString() => $"[{Left},{Right}]";

        public override int Magnitude()
            => (3 * Left.Magnitude()) + (2 * Right.Magnitude());

        public SnailPair Clone()
        {
            string raw = ToString();
            return ParseRaw(new[] { raw })[0];
        }

        public SnailValue? FindNearest(bool left)
        {
            var visited = new HashSet<SnailPair>() { this };
            return FindNearest(Parent, left, true, visited);

            static SnailValue? FindNearest(
                SnailPair? pair,
                bool searchLeft,
                bool movedUp,
                HashSet<SnailPair> visited)
            {
                if (pair is null)
                {
                    return null;
                }

                if (searchLeft && pair.Left is SnailValue leftValue)
                {
                    return leftValue;
                }

                if (!searchLeft && pair.Right is SnailValue rightValue)
                {
                    return rightValue;
                }

                visited.Add(pair);

                if (searchLeft && pair.Left is SnailPair leftPair &&
                    !visited.Contains(leftPair))
                {
                    visited.Add(leftPair);

                    bool direction = movedUp ? !searchLeft : searchLeft;

                    SnailValue? leftChild = FindNearest(leftPair, direction, false, visited);

                    if (leftChild is not null)
                    {
                        return leftChild;
                    }
                }

                if (!searchLeft && pair.Right is SnailPair rightPair &&
                    !visited.Contains(rightPair))
                {
                    visited.Add(rightPair);

                    bool direction = movedUp ? !searchLeft : searchLeft;

                    SnailValue? rightChild = FindNearest(rightPair, direction, false, visited);

                    if (rightChild is not null)
                    {
                        return rightChild;
                    }
                }

                return FindNearest(pair.Parent, searchLeft, true, visited);
            }
        }

        public SnailPair Reduce()
            => Reduce(this);

        private static SnailPair Reduce(SnailPair pair)
        {
            while (true)
            {
                if (FindExplode(pair, depth: 0, out var toExplode))
                {
                    SnailValue? left = toExplode.FindNearest(left: true);
                    SnailValue? right = toExplode.FindNearest(left: false);

                    if (left is not null)
                    {
                        left.Value += (toExplode.Left as SnailValue)!.Value;
                    }

                    if (right is not null)
                    {
                        right.Value += (toExplode.Right as SnailValue)!.Value;
                    }

                    SnailPair parent = toExplode.Parent!;
                    SnailValue exploded = new(0, parent);

                    if (parent.Left == toExplode)
                    {
                        parent.Left = exploded;
                    }
                    else
                    {
                        parent.Right = exploded;
                    }

                    continue;
                }

                if (FindSplit(pair, out var toSplit))
                {
                    SnailPair parent = toSplit.Parent!;
                    SnailPair split = toSplit.Split();

                    if (parent.Left == toSplit)
                    {
                        parent.Left = split;
                    }
                    else
                    {
                        parent.Right = split;
                    }

                    continue;
                }

                break;
            }

            return pair;

            static bool FindExplode(SnailPair pair, int depth, [NotNullWhen(true)] out SnailPair? value)
            {
                if (depth == 4)
                {
                    value = pair;
                    return true;
                }

                if (pair.Left is SnailPair leftPair)
                {
                    if (FindExplode(leftPair, depth + 1, out value))
                    {
                        return true;
                    }
                }

                if (pair.Right is SnailPair rightPair)
                {
                    if (FindExplode(rightPair, depth + 1, out value))
                    {
                        return true;
                    }
                }

                value = null;
                return false;
            }

            static bool FindSplit(SnailPair pair, [NotNullWhen(true)] out SnailValue? value)
            {
                const int SplitThreshold = 10;

                if (pair.Left is SnailValue leftValue &&
                    leftValue >= SplitThreshold)
                {
                    value = leftValue;
                    return true;
                }

                if (pair.Left is SnailPair leftPair &&
                    FindSplit(leftPair, out value))
                {
                    return true;
                }

                if (pair.Right is SnailValue rightValue &&
                    rightValue >= SplitThreshold)
                {
                    value = rightValue;
                    return true;
                }

                if (pair.Right is SnailPair rightPair &&
                    FindSplit(rightPair, out value))
                {
                    return true;
                }

                value = null;
                return false;
            }
        }
    }

    private sealed class SnailValue : SnailNumber
    {
        public SnailValue(int value, SnailPair parent)
        {
            Value = value;
            Parent = parent;
        }

        public int Value { get; set; }

        public static implicit operator int(SnailValue value)
            => value.Value;

        public override int Magnitude()
            => Value;

        public SnailPair Split()
        {
            (int div, int rem) = Math.DivRem(Value, 2);

            var result = new SnailPair() { Parent = Parent };

            result.Left = new SnailValue(div, result);
            result.Right = new SnailValue(div + rem, result);

            return result;
        }

        public override string ToString()
            => Value.ToString(CultureInfo.InvariantCulture);
    }
}
