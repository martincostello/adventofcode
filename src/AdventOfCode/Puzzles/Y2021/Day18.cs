// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

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
    /// Calculates the sum of the specified snail numbers.
    /// </summary>
    /// <param name="numbers">The snail numbers to sum.</param>
    /// <returns>
    /// The magnitude of the sum of the snail numbers.
    /// </returns>
    public static int Sum(IList<string> numbers)
    {
        List<SnailPair> pairs = ParseRaw(numbers);

        SnailPair sum = pairs.Aggregate((x, y) => (x + y).Reduce());

        return sum.Magnitude();
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

        MagnitudeOfSum = Sum(numbers);

        if (Verbose)
        {
            Logger.WriteLine("The magnitude of the final sum is {0:N0}.", MagnitudeOfSum);
        }

        return PuzzleResult.Create(MagnitudeOfSum);
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

                if (searchLeft && pair.Left is SnailPair leftPair && !visited.Contains(leftPair))
                {
                    visited.Add(leftPair);

                    bool direction = movedUp ? !searchLeft : searchLeft;

                    var leftChild = FindNearest(leftPair, direction, false, visited);

                    if (leftChild is not null)
                    {
                        return leftChild;
                    }
                }

                if (!searchLeft && pair.Right is SnailPair rightPair && !visited.Contains(rightPair))
                {
                    visited.Add(rightPair);

                    bool direction = movedUp ? !searchLeft : searchLeft;

                    var rightChild = FindNearest(rightPair, direction, false, visited);

                    if (rightChild is not null)
                    {
                        return rightChild;
                    }
                }

                return FindNearest(pair.Parent, searchLeft, true, visited);
            }
        }

        public SnailPair Reduce()
        {
            int unused = 0;
            return Reduce(this, null, null, ref unused)!;
        }

        private static SnailPair? Reduce(SnailPair number, SnailValue? left, SnailValue? right, ref int nested)
        {
            while (true)
            {
                if (nested == 4)
                {
                    if (left is not null)
                    {
                        left.Value += (number.Left as SnailValue)!;
                    }

                    if (right is not null)
                    {
                        right.Value += (number.Right as SnailValue)!;
                    }

                    return null;
                }

                if (number.Left is SnailPair leftPair)
                {
                    nested++;

                    try
                    {
                        SnailValue? childLeft = leftPair.FindNearest(left: true);
                        SnailValue? childRight = leftPair.FindNearest(left: false);

                        SnailPair? reduced = Reduce(leftPair, childLeft, childRight, ref nested);

                        if (reduced is null)
                        {
                            number.Left = new SnailValue(0, number);
                            continue;
                        }
                    }
                    finally
                    {
                        nested--;
                    }
                }

                if (number.Right is SnailPair rightPair)
                {
                    nested++;

                    try
                    {
                        SnailValue? childLeft = rightPair.FindNearest(left: true);
                        SnailValue? childRight = rightPair.FindNearest(left: false);

                        SnailPair? reduced = Reduce(rightPair, childLeft, childRight, ref nested);

                        if (reduced is null)
                        {
                            number.Right = new SnailValue(0, number);
                            continue;
                        }
                    }
                    finally
                    {
                        nested--;
                    }
                }

                if (number.Left is SnailValue leftValue && leftValue >= 10)
                {
                    number.Left = leftValue.Split();
                    continue;
                }

                if (number.Right is SnailValue rightValue && rightValue >= 10)
                {
                    number.Right = rightValue.Split();
                    continue;
                }

                break;
            }

            return number;
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

        public static implicit operator int(SnailValue value) => value.Value;

        public override int Magnitude() => Value;

        public SnailPair Split()
        {
            (int div, int rem) = Math.DivRem(Value, 2);

            var result = new SnailPair() { Parent = Parent };

            result.Left = new SnailValue(div, result);
            result.Right = new SnailValue(div + rem, result);

            return result;
        }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}
