// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 21, RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the canonocal list of ingredients that are allergens.
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
        IEnumerable<string> foods,
        CancellationToken cancellationToken = default)
    {
        // Based on https://github.com/DanaL/AdventOfCode/blob/master/2020/Day21.cs
        var parsedFoods = new List<(string[] Ingredients, string[] Allergens)>();

        foreach (string recipe in foods)
        {
            string[] split = recipe.Split('(');
            string[] ingredients = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] allergens = split[1][9..].TrimEnd(')').Split(", ");

            parsedFoods.Add((ingredients, allergens));
        }

        var allergenCandidates = new Dictionary<string, HashSet<string>>();
        var occurrences = new Dictionary<string, int>();

        foreach ((string[] ingredients, string[] allergens) in parsedFoods)
        {
            foreach (string allergen in allergens)
            {
                if (!allergenCandidates.TryGetValue(allergen, out HashSet<string>? candidates))
                {
                    allergenCandidates[allergen] = new HashSet<string>(ingredients);
                }
                else
                {
                    candidates.IntersectWith(ingredients);
                }
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

            foreach (string allergen in allergenCandidates.Keys.Where((p) => !knownAllergens.Contains(p)))
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

        int occurences = occurrences.Keys
            .Where((p) => !allergicIngredients.Contains(p))
            .Select((p) => occurrences[p])
            .Sum();

        var sortedAllergens = allergensForIngredients
            .OrderBy((p) => p.Value)
            .Select((p) => p.Key);

        string canonicalAllergens = string.Join(',', sortedAllergens);

        return (occurences, canonicalAllergens);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> recipies = await ReadResourceAsLinesAsync();

        (IngredientsWithNoAllergens, CanonicalAllergens) = GetIngredientsWithNoAllergens(recipies, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("Ingredients with no allergens appear {0} times.", IngredientsWithNoAllergens);
            Logger.WriteLine("The canonical allergens are: {0}.", CanonicalAllergens);
        }

        return PuzzleResult.Create(IngredientsWithNoAllergens, CanonicalAllergens);
    }
}
