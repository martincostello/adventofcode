// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 15, "Science for Hungry People", RequiresData = true)]
public sealed class Day15 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the highest total cookie score for the specified ingredients.
    /// </summary>
    /// <param name="collection">The ingredients.</param>
    /// <param name="calorieCount">The optional target calorie count.</param>
    /// <returns>
    /// The highest total cookie score for the specified ingredients.
    /// </returns>
    internal static int GetHighestTotalCookieScore(IList<string> collection, int? calorieCount = null)
    {
        // Parse the ingredients
        var ingredients = new Ingredient[collection.Count];

        for (int i = 0; i < collection.Count; i++)
        {
            ingredients[i] = Ingredient.Parse(collection[i]);
        }

        // Get the possible combinations of teaspoons for each of the ingredients
        var teaspoonPermutations = GetTeaspoonPermutations(ingredients.Length);

        // Calculate the total score for each possible recipe
        int bestScore = Unsolved;

        foreach (int[] teaspoons in teaspoonPermutations)
        {
            int thisScore = GetRecipeScore(teaspoons, ingredients, calorieCount);

            if (thisScore > bestScore)
            {
                bestScore = thisScore;
            }
        }

        return bestScore;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (ingredients, logger, _) =>
            {
                const int CalorieTarget = 500;

                int highestTotalCookieScore = GetHighestTotalCookieScore(ingredients);
                int highestTotalCookieScoreWith500Calories = GetHighestTotalCookieScore(ingredients, calorieCount: CalorieTarget);

                if (logger is { })
                {
                    logger.WriteLine("The highest total cookie score is {0:N0}.", highestTotalCookieScore);
                    logger.WriteLine("The highest total cookie score for a cookie with {0} calories is {1:N0}.", CalorieTarget, highestTotalCookieScoreWith500Calories);
                }

                return (highestTotalCookieScore, highestTotalCookieScoreWith500Calories);
            },
            cancellationToken);
    }

    /// <summary>
    /// Gets the permutation of teaspoon values for a recipe with the specified number of ingredients.
    /// </summary>
    /// <param name="recipeCount">The number of ingredients to get the permutations for.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> containing the number of teaspoons to use of an ingredient where
    /// at least one spoon of ingredient is required and no more than 100 teaspoons can be used.
    /// </returns>
    private static List<int[]> GetTeaspoonPermutations(int recipeCount)
    {
        const int MaxTeaspoons = 100;

        var result = new List<int[]>();
        int[] seed = new int[recipeCount];

        GetTeaspoonPermutations(seed, 0, MaxTeaspoons, result);

        return result;
    }

    /// <summary>
    /// Gets the permutation of teaspoon values for a recipe with the specified number of ingredients.
    /// </summary>
    /// <param name="seed">The seed partial teaspoon count to use to calculate further permutations.</param>
    /// <param name="index">The index of the current teaspoon to calculate the values for.</param>
    /// <param name="remaining">The number of teaspoons remaining to be used.</param>
    /// <param name="result">The collection to add the permutations to.</param>
    private static void GetTeaspoonPermutations(int[] seed, int index, int remaining, List<int[]> result)
    {
        if (index == seed.Length - 1)
        {
            // This is the last ingredient so the only possible value is the remainder
            seed[index] = remaining;
            result.Add((int[])seed.Clone());
            return;
        }

        // Each ingredient must have at least one teaspoon, so the upper bound
        // is the maximum, less the number already used minus enough so that
        // any remaining ingredients get at least one teaspoon.
        int upperBound = remaining - (seed.Length - index - 1);

        // Zero is not considered as that would create a score of zero, and the
        // highest score is required so such recipes can be immediately discounted.
        for (int i = 1; i <= upperBound; i++)
        {
            seed[index] = i;
            GetTeaspoonPermutations(seed, index + 1, remaining - i, result);
        }
    }

    /// <summary>
    /// Gets the score for the specified recipe that optionally has the specified number of calories.
    /// </summary>
    /// <param name="teaspoons">The number of teaspoons of each ingredient in the recipe.</param>
    /// <param name="ingredients">The ingredients.</param>
    /// <param name="calorieCount">The optional target calorie count.</param>
    /// <returns>
    /// The total score for the specified recipe.
    /// </returns>
    private static int GetRecipeScore(
        ReadOnlySpan<int> teaspoons,
        ReadOnlySpan<Ingredient> ingredients,
        int? calorieCount)
    {
        int capacity = 0;
        int durability = 0;
        int flavor = 0;
        int texture = 0;
        int calories = 0;

        for (int i = 0; i < teaspoons.Length; i++)
        {
            int count = teaspoons[i];

            if (count < 1)
            {
                continue;
            }

            Ingredient ingredient = ingredients[i];

            calories += ingredient.Calories * count;
            capacity += ingredient.Capacity * count;
            durability += ingredient.Durability * count;
            flavor += ingredient.Flavor * count;
            texture += ingredient.Texture * count;
        }

        // Negative properties cause the score to become zero
        int score = capacity < 0 || durability < 0 || flavor < 0 || texture < 0
            ? 0
            : capacity * durability * flavor * texture;

        if (calorieCount.HasValue && calories != calorieCount.GetValueOrDefault())
        {
            // Does not have the target number of calories, so discount
            score = 0;
        }

        return score;
    }

    /// <summary>
    /// A structure representing an ingredient.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="calories">The calories.</param>
    /// <param name="capacity">The capacity.</param>
    /// <param name="durability">The durability.</param>
    /// <param name="flavor">The flavor.</param>
    /// <param name="texture">The texture.</param>
    private readonly struct Ingredient(string name, int calories, int capacity, int durability, int flavor, int texture)
    {
        /// <summary>
        /// Gets or sets the name of the ingredient.
        /// </summary>
        internal readonly string Name = name;

        /// <summary>
        /// Gets the calories.
        /// </summary>
        internal readonly int Calories = calories;

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        internal readonly int Capacity = capacity;

        /// <summary>
        /// Gets the durability.
        /// </summary>
        internal readonly int Durability = durability;

        /// <summary>
        /// Gets the flavor.
        /// </summary>
        internal readonly int Flavor = flavor;

        /// <summary>
        /// Gets the texture.
        /// </summary>
        internal readonly int Texture = texture;

        /// <summary>
        /// Parses the specified <see cref="string"/> to an instance of <see cref="Ingredient"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An instance of <see cref="Ingredient"/> created from <paramref name="value"/>.
        /// </returns>
        internal static Ingredient Parse(string value)
        {
            string[] split = value.Split(':');
            string name = split[0];

            split = string.Join(string.Empty, split, 1, split.Length - 1)
                .Split(',', StringSplitOptions.TrimEntries);

            (_, string rawCapacity) = split[0].Bifurcate(' ');
            (_, string rawDurability) = split[1].Bifurcate(' ');
            (_, string rawFlavor) = split[2].Bifurcate(' ');
            (_, string rawTexture) = split[3].Bifurcate(' ');
            (_, string rawCalories) = split[4].Bifurcate(' ');

            return new(
                name,
                Parse<int>(rawCalories),
                Parse<int>(rawCapacity),
                Parse<int>(rawDurability),
                Parse<int>(rawFlavor),
                Parse<int>(rawTexture));
        }
    }
}
