﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 21, "Allergen Assessment", RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the canonical list of ingredients that are allergens.
    /// </summary>
    public string CanonicalAllergens { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the number of times ingredients with no allergens appear.
    /// </summary>
    public int IngredientsWithNoAllergens { get; private set; }

    /// <summary>
    /// Gets the number of times ingredients with no allergens appear in the specified foods
    /// and the list of ingredients which are the allergens.
    /// </summary>
    /// <param name="foods">The foods to check for allergens.</param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// The number of times ingredients with no allergens appear in the food and
    /// the canonical sorted list of ingredients which are allergens.
    /// </returns>
    public static (int Occurences, string CanonicalAllergens) GetIngredientsWithNoAllergens(
        IList<string> foods,
        CancellationToken cancellationToken = default)
    {
        // Based on https://github.com/DanaL/AdventOfCode/blob/main/2020/Day21.cs
        var parsedFoods = new (string[] Ingredients, string[] Allergen)[foods.Count];

        for (int i = 0; i < foods.Count; i++)
        {
            (string first, string second) = foods[i].Bifurcate('(');
            string[] ingredients = first.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] allergens = second[9..].TrimEnd(')').Split(", ");

            parsedFoods[i] = (ingredients, allergens);
        }

        var allergenCandidates = new Dictionary<string, HashSet<string>>();
        var occurrences = new Dictionary<string, int>();

        foreach ((string[] ingredients, string[] allergens) in parsedFoods)
        {
            foreach (string allergen in allergens)
            {
                var candidates = allergenCandidates.GetOrAdd(allergen, () => new HashSet<string>(ingredients));
                candidates.IntersectWith(ingredients);
            }

            foreach (string ingredient in ingredients)
            {
                occurrences.AddOrIncrement(ingredient, 1);
            }
        }

        var knownAllergens = new HashSet<string>();
        var allergicIngredients = new HashSet<string>();
        var allergensForIngredients = new Dictionary<string, string>();

        while (!cancellationToken.IsCancellationRequested)
        {
            bool hasUnidentified = false;

            foreach (string allergen in allergenCandidates.Keys.Except(knownAllergens))
            {
                HashSet<string> candidates = allergenCandidates[allergen];

                if (candidates.Count == 1)
                {
                    string ingredient = candidates.First();

                    allergicIngredients.Add(ingredient);

                    foreach (string other in allergenCandidates.Keys)
                    {
                        allergenCandidates[other].Remove(ingredient);
                    }

                    knownAllergens.Add(allergen);
                    allergensForIngredients[ingredient] = allergen;
                }
                else
                {
                    hasUnidentified = true;
                }
            }

            if (!hasUnidentified)
            {
                break;
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        int count = occurrences.Keys
            .Where((p) => !allergicIngredients.Contains(p))
            .Select((p) => occurrences[p])
            .Sum();

        var sortedAllergens = allergensForIngredients
            .OrderBy((p) => p.Value)
            .Select((p) => p.Key);

        string canonicalAllergens = string.Join(',', sortedAllergens);

        return (count, canonicalAllergens);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var recipes = await ReadResourceAsLinesAsync(cancellationToken);

        (IngredientsWithNoAllergens, CanonicalAllergens) = GetIngredientsWithNoAllergens(recipes, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("Ingredients with no allergens appear {0} times.", IngredientsWithNoAllergens);
            Logger.WriteLine("The canonical allergens are: {0}.", CanonicalAllergens);
        }

        return PuzzleResult.Create(IngredientsWithNoAllergens, CanonicalAllergens);
    }
}
