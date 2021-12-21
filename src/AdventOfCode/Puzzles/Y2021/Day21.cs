// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 21, "Dirac Dice", RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the product of the score of the losing player and
    /// the number of times the die was rolled during the game.
    /// </summary>
    public int Outcome { get; private set; }

    /// <summary>
    /// Plays a game of Dirac Dice.
    /// </summary>
    /// <param name="players">The initial starting tiles of the players.</param>
    /// <returns>
    /// The product of the score of the losing player and
    /// the number of times the die was rolled during the game.
    /// </returns>
    public static int Play(IList<string> players)
    {
        const string Prefix = "Player X starting position: ";

        int position1 = Parse<int>(players[0][Prefix.Length..]);
        int position2 = Parse<int>(players[1][Prefix.Length..]);

        bool player1Turn = true;
        int rolls = 0;

        int score1 = 0;
        int score2 = 0;

        var deterministicDie = Roll3();
        using var die = deterministicDie.GetEnumerator();

        const long WinningScore = 1000;

        while (score1 < WinningScore && score2 < WinningScore && die.MoveNext())
        {
            int roll = die.Current;

            if (player1Turn)
            {
                score1 += Move(ref position1, roll);
            }
            else
            {
                score2 += Move(ref position2, roll);
            }

            player1Turn = !player1Turn;
            rolls++;
        }

        int losingScore = Math.Min(score1, score2);

        return losingScore * rolls * 3;

        static int Move(ref int position, int roll)
            => position = ((position - 1 + roll) % 10) + 1;

        static IEnumerable<int> Roll3()
        {
            int value = 1;

            while (true)
            {
                int result = value;
                result += Increment(ref value);
                result += Increment(ref value);

                yield return result;

                Increment(ref value);
            }

            static int Increment(ref int value)
            {
                const int Faces = 100;
                return value = (value % Faces) + 1;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> players = await ReadResourceAsLinesAsync();

        Outcome = Play(players);

        if (Verbose)
        {
            Logger.WriteLine(
                "The product of the score of the losing player and the number of times the die was rolled during the game is {0:N0}.",
                Outcome);
        }

        return PuzzleResult.Create(Outcome);
    }
}
