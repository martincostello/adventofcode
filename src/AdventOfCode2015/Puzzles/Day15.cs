// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/15</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day15 : IPuzzle
    {
        /// <summary>
        /// Gets the highest total cookie score.
        /// </summary>
        internal int HighestTotalCookieScore { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            string[] ingredients = File.ReadAllLines(args[0]);

            HighestTotalCookieScore = GetHighestTotalCookieScore(ingredients);

            Console.WriteLine("The highest total cookie score is {0:N0}.", HighestTotalCookieScore);

            return 0;
        }

        /// <summary>
        /// Returns the highest total cookie score for the specified ingredients.
        /// </summary>
        /// <param name="collection">The ingredients.</param>
        /// <returns>
        /// The highest total cookie score for the specified ingredients.
        /// </returns>
        internal static int GetHighestTotalCookieScore(ICollection<string> collection)
        {
            var ingredients = collection
                .Select(Ingredient.Parse)
                .ToDictionary((p) => p.Name, (p) => p);

            var recipies = new List<IDictionary<string, int>>();

            // TODO Recipe generation is not generating the correct ingredient
            // permutations when the number of ingredients is more than two.
            foreach (string ingredient in ingredients.Keys)
            {
                for (int i = 0; i < 100; i++)
                {
                    var recipe = new Dictionary<string, int>();
                    recipe[ingredient] = i;

                    var otherIngredients = ingredients.Keys
                        .Where((p) => p != ingredient)
                        .ToArray();

                    for (int k = 0; recipe.Values.Sum() + k <= 100; k++)
                    {
                        for (int j = 0; j < otherIngredients.Length; j++)
                        {
                            recipe[otherIngredients[j]] = 100 - recipe.Values.Sum();
                        }
                    }

                    recipies.Add(recipe);
                }
            }

            var scores = new List<int>();

            foreach (var recipe in recipies)
            {
                int score = GetRecipeScore(recipe, ingredients);
                scores.Add(score);
            }

            return scores.Max();
        }

        /// <summary>
        /// Gets the score for the specified recipe.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        /// <param name="ingredients">The ingredients.</param>
        /// <returns>
        /// The total score for the specified recipe.
        /// </returns>
        private static int GetRecipeScore(IDictionary<string, int> recipe, IDictionary<string, Ingredient> ingredients)
        {
            int capacity = 0;
            int durability = 0;
            int flavor = 0;
            int texture = 0;

            foreach (var item in recipe.Where((p) => p.Value > 0))
            {
                var ingredient = ingredients[item.Key];

                capacity += ingredient.Capacity * item.Value;
                durability += ingredient.Durability * item.Value;
                flavor += ingredient.Flavor * item.Value;
                texture += ingredient.Texture * item.Value;
            }

            int score;

            if (capacity < 0 || durability < 0 || flavor < 0 || texture < 0)
            {
                score = 0;
            }
            else
            {
                score = capacity * durability * flavor * texture;
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
            internal string Name { get; set; }

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
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Will be used in part 2.")]
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

                Ingredient result = new Ingredient()
                {
                    Name = split[0],
                };

                split = string.Join(string.Empty, split, 1, split.Length - 1).Split(',');

                result.Calories = int.Parse(split[4].Trim().Split(' ')[1], CultureInfo.InvariantCulture);
                result.Capacity = int.Parse(split[0].Trim().Split(' ')[1], CultureInfo.InvariantCulture);
                result.Durability = int.Parse(split[1].Trim().Split(' ')[1], CultureInfo.InvariantCulture);
                result.Flavor = int.Parse(split[2].Trim().Split(' ')[1], CultureInfo.InvariantCulture);
                result.Texture = int.Parse(split[3].Trim().Split(' ')[1], CultureInfo.InvariantCulture);

                return result;
            }
        }
    }
}
