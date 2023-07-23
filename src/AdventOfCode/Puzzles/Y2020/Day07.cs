// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 07, "Handy Haversacks", MinimumArguments = 1, RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the number of bag colors that can eventually contain at least one bag of the given color.
    /// </summary>
    public int BagColorsThatCanContainColor { get; private set; }

    /// <summary>
    /// Gets the number of bags required inside the specified top-level bag to allow it be carried.
    /// </summary>
    public int BagsInsideBag { get; private set; }

    /// <summary>
    /// Gets the number of bag colors that can eventually contain at least one bag of the specified colors.
    /// </summary>
    /// <param name="rules">A collection of bag-colour relationship rules.</param>
    /// <param name="color">The color to get the number of bag colors for.</param>
    /// <returns>
    /// The number of bag colors that can eventually contain at least one bag of the color specified by <paramref name="color"/>.
    /// </returns>
    public static int GetBagColorsThatCouldContainColor(ICollection<string> rules, string color)
    {
        Dictionary<string, Dictionary<string, int>> colors = ParseRules(rules);

        colors.Remove(color);

        int result = 0;

        foreach (var bag in colors)
        {
            var path = new Stack<string>();
            path.Push(bag.Key);

            if (ContainsPathToTarget(path, color))
            {
                result++;
            }
        }

        return result;

        bool ContainsPathToTarget(Stack<string> path, string targetColor)
        {
            if (!colors.TryGetValue(path.Peek(), out var self))
            {
                return false;
            }

            if (self.ContainsKey(targetColor))
            {
                return true;
            }

            bool foundPath = false;

            foreach (var bag in self)
            {
                if (path.Contains(bag.Key))
                {
                    continue;
                }

                path.Push(bag.Key);

                foundPath = ContainsPathToTarget(path, targetColor);

                path.Pop();

                if (foundPath)
                {
                    break;
                }
            }

            return foundPath;
        }
    }

    /// <summary>
    /// Gets the number of bags required inside the specified top-level bag to allow it be carried.
    /// </summary>
    /// <param name="rules">A collection of bag-colour relationship rules.</param>
    /// <param name="color">The color to get the count of bags inside.</param>
    /// <returns>
    /// The number of bags required inside the top-level bag of the specified color to allow it be carried.
    /// </returns>
    public static int GetInsideBagCount(ICollection<string> rules, string color)
    {
        Dictionary<string, Dictionary<string, int>> colors = ParseRules(rules);

        var path = new Stack<string>();
        path.Push(color);

        return CountChildren(path);

        int CountChildren(Stack<string> path)
        {
            if (!colors.TryGetValue(path.Peek(), out var self))
            {
                return 0;
            }

            int count = self.Values.Sum();

            foreach (var bag in self)
            {
                if (path.Contains(bag.Key))
                {
                    continue;
                }

                path.Push(bag.Key);

                count += CountChildren(path) * bag.Value;

                path.Pop();
            }

            return count;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string color = args[0];

        var rules = await ReadResourceAsLinesAsync(cancellationToken);

        BagColorsThatCanContainColor = GetBagColorsThatCouldContainColor(rules, color);
        BagsInsideBag = GetInsideBagCount(rules, color);

        if (Verbose)
        {
            Logger.WriteLine("The number of bags that can contain a {0} bag is {1}.", color, BagColorsThatCanContainColor);
            Logger.WriteLine("The number of bags contained in a {0} bag is {1}.", color, BagsInsideBag);
        }

        return PuzzleResult.Create(BagColorsThatCanContainColor, BagsInsideBag);
    }

    /// <summary>
    /// Parses the specified rules for bag contents and counts.
    /// </summary>
    /// <param name="rules">The rules to parse.</param>
    /// <returns>
    /// The number of color of bags that each color of bag may contain.
    /// </returns>
    private static Dictionary<string, Dictionary<string, int>> ParseRules(ICollection<string> rules)
    {
        var colors = new Dictionary<string, Dictionary<string, int>>();

        foreach (string rule in rules)
        {
            const string Delimiter = " bags contain ";

            int index = rule.AsSpan().IndexOf(Delimiter);

            string thisColor = rule[0..index];
            string contents = rule[(index + Delimiter.Length)..^1];

            string[] split = contents.Split(", ");

            var bagColors = colors[thisColor] = new Dictionary<string, int>();

            foreach (string bag in split)
            {
                if (bag is "no other bags")
                {
                    break;
                }

                string[] otherBag = bag.Split(' ');

                int count = Parse<int>(otherBag[0]);
                string otherColor = string.Join(' ', otherBag[1..^1]);

                bagColors[otherColor] = count;
            }
        }

        return colors;
    }
}
