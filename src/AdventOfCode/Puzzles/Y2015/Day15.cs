// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/15</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 15, RequiresData = true)]
    public sealed class Day15 : Puzzle
    {
        /// <summary>
        /// Gets the highest total cookie score.
        /// </summary>
        internal int HighestTotalCookieScore { get; private set; }

        /// <summary>
        /// Gets the highest total cookie score that has exactly 500 calories.
        /// </summary>
        internal int HighestTotalCookieScoreWith500Calories { get; private set; }

        /// <summary>
        /// Returns the highest total cookie score for the specified ingredients.
        /// </summary>
        /// <param name="collection">The ingredients.</param>
        /// <param name="calorieCount">The optional target calorie count.</param>
        /// <returns>
        /// The highest total cookie score for the specified ingredients.
        /// </returns>
        internal static int GetHighestTotalCookieScore(ICollection<string> collection, int? calorieCount = null)
        {
            // Parse the ingredients
            var ingredientProperties = collection
                .Select(Ingredient.Parse)
                .ToDictionary((p) => p.Name, (p) => p);

            // Get the possible combinations of teaspoons for each of the ingredients
            IList<IList<int>> teaspoonPermutations = GetTeaspoonPermutations(ingredientProperties.Count);

            // Get all the permutations which the ingredients could be ordered by
            var ingredientPermutations = Maths.GetPermutations(ingredientProperties.Keys).ToList();

            var recipies = new List<IDictionary<string, int>>(ingredientPermutations.Count * teaspoonPermutations.Count);

            // For each permutation of ingredients, create a recipe for each
            // permutation of the number of teaspoons of each one there is.
            foreach (var ordering in ingredientPermutations)
            {
                IList<string> ingredients = ordering.ToList();

                foreach (IList<int> teaspoons in teaspoonPermutations)
                {
                    var recipe = new Dictionary<string, int>(ingredients.Count);

                    for (int i = 0; i < ingredients.Count; i++)
                    {
                        recipe[ingredients[i]] = teaspoons[i];
                    }

                    recipies.Add(recipe);
                }
            }

            // Calculate the total score for each possible recipe
            var scores = new List<int>(recipies.Count);

            foreach (var recipe in recipies)
            {
                int score = GetRecipeScore(recipe, ingredientProperties, calorieCount);
                scores.Add(score);
            }

            // Return the best recipe
            return scores.Max();
        }

        /// <inheritdoc />
        protected override object[] SolveCore(string[] args)
        {
            IList<string> ingredients = ReadResourceAsLines();

            HighestTotalCookieScore = GetHighestTotalCookieScore(ingredients);
            HighestTotalCookieScoreWith500Calories = GetHighestTotalCookieScore(ingredients, 500);

            if (Verbose)
            {
                Logger.WriteLine("The highest total cookie score is {0:N0}.", HighestTotalCookieScore);
                Logger.WriteLine("The highest total cookie score for a cookie with 500 calories is {0:N0}.", HighestTotalCookieScoreWith500Calories);
            }

            return new object[]
            {
                HighestTotalCookieScore,
                HighestTotalCookieScoreWith500Calories,
            };
        }

        /// <summary>
        /// Gets the permutation of teaspoon values for a recipe with the specified number of ingredients.
        /// </summary>
        /// <param name="recipeCount">The number of ingredients to get the permutations for.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the number of teaspoons to use of an ingredient where
        /// at least one spoon of ingredient is required and no more than 100 teaspoons can be used.
        /// </returns>
        private static IList<IList<int>> GetTeaspoonPermutations(int recipeCount)
            => GetTeaspoonPermutations(Array.Empty<int>(), 0, recipeCount);

        /// <summary>
        /// Gets the permutation of teaspoon values for a recipe with the specified number of ingredients.
        /// </summary>
        /// <param name="seed">The seed partial teaspoon count to use to calculate further premutations.</param>
        /// <param name="index">The index of the current teaspoon to calculate the values for.</param>
        /// <param name="count">The number of ingredients to get the permutations for.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the number of teaspoons to use of an ingredient where
        /// at least one spoon of ingredient is required and no more than 100 teaspoons can be used.
        /// </returns>
        private static IList<IList<int>> GetTeaspoonPermutations(IList<int> seed, int index, int count)
        {
            const int MaxTeaspoons = 100;

            // Holds the permutations of teaspoons for the number of ingredients at this index (i.e. count - index)
            IList<IList<int>> thisLevel = new List<IList<int>>(seed.Count);

            if (seed.Count < index + 1)
            {
                if (index == count - 1)
                {
                    // This is the last ingredient so the only possible value is the remainder
                    var next = new List<int>(seed)
                    {
                        MaxTeaspoons - seed.Sum(),
                    };

                    thisLevel.Add(next);
                }
                else
                {
                    // Each ingredient must have at least one teaspoon, so the upper bound
                    // is the maximum, less the number already used minus enough so that
                    // any remaining ingredients get at least one teaspoon.
                    int upperBound = MaxTeaspoons - seed.Sum() - count + index + 1;

                    // Zero is not considered as that would create a score of zero, and we're
                    // looking for the highest score so we can immediately discount such recipes.
                    for (int i = 1; i < upperBound; i++)
                    {
                        var next = new List<int>(seed)
                        {
                            i,
                        };

                        thisLevel.Add(next);
                    }
                }
            }

            if (thisLevel.Max((p) => p.Count) == count)
            {
                // We've got no more ingredients to find permutations for at this index
                return thisLevel;
            }

            // Find the permutations for the next ingredient from the ones we just found
            var result = new List<IList<int>>(thisLevel.Count);

            foreach (var amount in thisLevel)
            {
                result.AddRange(GetTeaspoonPermutations(amount, index + 1, count));
            }

            return result;
        }

        /// <summary>
        /// Gets the score for the specified recipe that optionally has the specified number of calories.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        /// <param name="ingredients">The ingredients.</param>
        /// <param name="calorieCount">The optional target calorie count.</param>
        /// <returns>
        /// The total score for the specified recipe.
        /// </returns>
        private static int GetRecipeScore(
            IDictionary<string, int> recipe,
            IDictionary<string, Ingredient> ingredients,
            int? calorieCount)
        {
            int capacity = 0;
            int durability = 0;
            int flavor = 0;
            int texture = 0;
            int calories = 0;

            foreach (var item in recipe.Where((p) => p.Value > 0))
            {
                Ingredient ingredient = ingredients[item.Key];

                calories += ingredient.Calories * item.Value;
                capacity += ingredient.Capacity * item.Value;
                durability += ingredient.Durability * item.Value;
                flavor += ingredient.Flavor * item.Value;
                texture += ingredient.Texture * item.Value;
            }

            int score;

            // Negative properties cause the score to become zero
            if (capacity < 0 || durability < 0 || flavor < 0 || texture < 0)
            {
                score = 0;
            }
            else
            {
                score = capacity * durability * flavor * texture;
            }

            if (calorieCount.HasValue && calories != calorieCount.Value)
            {
                // Does not have the target number of calories, so discount
                score = 0;
            }

            return score;
        }

        /// <summary>
        /// A class representing an ingredient. This class cannot be inherited.
        /// </summary>
        private sealed class Ingredient
        {
            /// <summary>
            /// Gets or sets the name of the ingredient.
            /// </summary>
            internal string Name { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the capacity.
            /// </summary>
            internal int Capacity { get; set; }

            /// <summary>
            /// Gets or sets the durability.
            /// </summary>
            internal int Durability { get; set; }

            /// <summary>
            /// Gets or sets the flavor.
            /// </summary>
            internal int Flavor { get; set; }

            /// <summary>
            /// Gets or sets the texture.
            /// </summary>
            internal int Texture { get; set; }

            /// <summary>
            /// Gets or sets the calories.
            /// </summary>
            internal int Calories { get; set; }

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

                var result = new Ingredient()
                {
                    Name = split[0],
                };

                split = string.Join(string.Empty, split, 1, split.Length - 1).Split(',');

                result.Calories = ParseInt32(split[4].Trim().Split(' ')[1]);
                result.Capacity = ParseInt32(split[0].Trim().Split(' ')[1]);
                result.Durability = ParseInt32(split[1].Trim().Split(' ')[1]);
                result.Flavor = ParseInt32(split[2].Trim().Split(' ')[1]);
                result.Texture = ParseInt32(split[3].Trim().Split(' ')[1]);

                return result;
            }
        }
    }
}
