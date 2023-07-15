// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 21, "Dirac Dice", RequiresData = true, IsSlow = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the product of the score of the losing player and
    /// the number of times the die was rolled during the game
    /// for the practice game with the deterministic die.
    /// </summary>
    public int PracticeOutcome { get; private set; }

    /// <summary>
    /// Gets the number of universes in which the winning
    /// player wins for the real game using the Dirac die.
    /// </summary>
    public long WinningUniverses { get; private set; }

    /// <summary>
    /// Plays a practice game of Dirac Dice.
    /// </summary>
    /// <param name="players">The initial starting tiles of the players.</param>
    /// <returns>
    /// The product of the score of the losing player and
    /// the number of times the die was rolled during the game.
    /// </returns>
    public static int PlayPractice(IList<string> players)
    {
        (Player player1, Player player2) = Parse(players);

        bool player1Turn = true;
        int rolls = 0;

        var deterministicDie = Roll3D100();
        using var die = deterministicDie.GetEnumerator();

        const long WinningScore = 1_000;

        while (player1.Score < WinningScore && player2.Score < WinningScore && die.MoveNext())
        {
            int roll = die.Current;

            if (player1Turn)
            {
                player1 = Move(player1, roll);
            }
            else
            {
                player2 = Move(player2, roll);
            }

            player1Turn = !player1Turn;
            rolls++;
        }

        int losingScore = Math.Min(player1.Score, player2.Score);

        return losingScore * rolls * 3;

        static IEnumerable<int> Roll3D100()
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

    /// <summary>
    /// Plays a real game of Dirac Dice.
    /// </summary>
    /// <param name="players">The initial starting tiles of the players.</param>
    /// <returns>
    /// The number of universes in which the winning player wins.
    /// </returns>
    public static long Play(IList<string> players)
    {
        (Player player1, Player player2) = Parse(players);

        var states = new Dictionary<(Player P1, Player P2, int NextDie, int Rolls), (long Wins1, long Wins2)>();

        (long wins1, long wins2) = Play((player1, player2, 0, -1));

        return Math.Max(wins1, wins2);

        (long Wins1, long Wins2) Play((Player P1, Player P2, int NextDie, int Rolls) state)
        {
            if (states.TryGetValue(state, out var score))
            {
                // We already know the result for this game state
                return score;
            }

            (Player player1, Player player2, int nextDie, int rolls) = state;

            int nextPosition = Move(player1.Position, nextDie);

            const int RollLimit = 2;
            const int WinningScore = 21;

            if (rolls == RollLimit)
            {
                // Move the player and update their score
                player1 = new(nextPosition, player1.Score + nextPosition);

                if (player1.Score >= WinningScore)
                {
                    return states[state] = (1, 0);
                }
            }
            else
            {
                // Just move the player
                player1 = new(nextPosition, player1.Score);
            }

            bool swappedPlayers = false;

            if (rolls == RollLimit)
            {
                rolls = 0;
                swappedPlayers = true;
                (player1, player2) = (player2, player1);
            }
            else
            {
                rolls++;
            }

            long wins1 = 0;
            long wins2 = 0;

            for (int i = 1; i <= 3; i++)
            {
                (long w1, long w2) = Play((player1, player2, i, rolls));

                if (swappedPlayers)
                {
                    (w1, w2) = (w2, w1);
                }

                wins1 += w1;
                wins2 += w2;
            }

            return states[state] = (wins1, wins2);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var players = await ReadResourceAsLinesAsync(cancellationToken);

        PracticeOutcome = PlayPractice(players);
        WinningUniverses = Play(players);

        if (Verbose)
        {
            Logger.WriteLine(
                "The product of the score of the losing player and the number of times the die was rolled during the game is {0:N0}.",
                PracticeOutcome);

            Logger.WriteLine(
                "The winning player wins in {0:N0} universes during the real game.",
                WinningUniverses);
        }

        return PuzzleResult.Create(PracticeOutcome, WinningUniverses);
    }

    private static Player Move(Player player, int roll)
    {
        int position = Move(player.Position, roll);
        return new(position, player.Score + position);
    }

    private static int Move(int position, int roll)
        => ((position - 1 + roll) % 10) + 1;

    private static (Player Player1, Player Player2) Parse(IList<string> players)
    {
        const string Prefix = "Player X starting position: ";

        int position1 = Parse<int>(players[0][Prefix.Length..]);
        int position2 = Parse<int>(players[1][Prefix.Length..]);

        return (new(position1), new(position2));
    }

    private readonly struct Player(int position, int score = 0)
    {
        public readonly int Position = position;
        public readonly int Score = score;

        public readonly void Deconstruct(out int position, out int score)
        {
            position = Position;
            score = Score;
        }
    }
}
