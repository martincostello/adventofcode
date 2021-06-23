// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/2</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 02, RequiresData = true)]
    public sealed class Day02 : Puzzle
    {
        /// <summary>
        /// Gets the number of valid passwords using policy version 1.
        /// </summary>
        public int ValidPasswordsV1 { get; private set; }

        /// <summary>
        /// Gets the number of valid passwords using policy version 2.
        /// </summary>
        public int ValidPasswordsV2 { get; private set; }

        /// <summary>
        /// Gets whether the specified password is valid as determined by its criteria.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <param name="policyVersion">The password policy version to use.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is valid; otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsPasswordValid(string value, int policyVersion)
        {
            string[] parts = value.Split(' ');
            string[] numbers = parts[0].Split('-');

            int firstNumber = ParseInt32(numbers[0]);
            int secondNumber = ParseInt32(numbers[1]);

            char requiredCharacter = parts[1][0];
            string password = parts[2];

            if (policyVersion == 1)
            {
                int minimumCount = firstNumber;
                int maximumCount = secondNumber;

                int count = password.Count((p) => p == requiredCharacter);
                return count >= minimumCount && count <= maximumCount;
            }
            else
            {
                int index1 = firstNumber - 1;
                int index2 = secondNumber - 1;

                return password[index1] == requiredCharacter ^ password[index2] == requiredCharacter;
            }
        }

        /// <summary>
        /// Gets the number of valid passwords in the specified list.
        /// </summary>
        /// <param name="values">The values to validate passwords against.</param>
        /// <param name="policyVersion">The password policy version to use.</param>
        /// <returns>
        /// The number of valid passwords in the specified set.
        /// </returns>
        public static int GetValidPasswordCount(IEnumerable<string> values, int policyVersion)
        {
            return values
                .Where((p) => IsPasswordValid(p, policyVersion))
                .Count();
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> values = await ReadResourceAsLinesAsync();

            ValidPasswordsV1 = GetValidPasswordCount(values, policyVersion: 1);
            ValidPasswordsV2 = GetValidPasswordCount(values, policyVersion: 2);

            if (Verbose)
            {
                Logger.WriteLine("There are {0} valid passwords using policy version 1.", ValidPasswordsV1);
                Logger.WriteLine("There are {0} valid passwords using policy version 2.", ValidPasswordsV2);
            }

            return PuzzleResult.Create(ValidPasswordsV1, ValidPasswordsV2);
        }
    }
}
