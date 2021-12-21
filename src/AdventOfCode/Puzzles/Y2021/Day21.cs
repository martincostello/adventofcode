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

        const long WinningScore = 1000;

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

        var states = new Dictionary<(Player P1, Player P2, int NextDice, int Rolls, bool MovePlayer1), (long Wins1, long Wins2)>();

        (long wins1, long wins2) = Play((player1, player2, 0, -1, true));

        return Math.Max(wins1, wins2);

        (long Wins1, long Wins2) Play((Player P1, Player P2, int NextDice, int Rolls, bool MovePlayer1) state)
        {
            if (states.TryGetValue(state, out var score))
            {
                return score;
            }

            const int RollLimit = 2;
            const int WinningScore = 21;

            (Player player1, Player player2, int nextDice, int rolls, bool movePlayer1) = state;

            if (movePlayer1)
            {
                int pos1 = Move(player1.Position, nextDice);

                if (rolls == RollLimit)
                {
                    player1 = new(pos1, player1.Score + pos1);

                    if (player1.Score >= WinningScore)
                    {
                        return states[state] = (1, 0);
                    }
                }
                else
                {
                    player1 = new(pos1, player1.Score);
                }
            }
            else
            {
                int pos2 = Move(player2.Position, nextDice);

                if (rolls == RollLimit)
                {
                    player2 = new(pos2, player2.Score + pos2);

                    if (player2.Score >= WinningScore)
                    {
                        return states[state] = (0, 1);
                    }
                }
                else
                {
                    player2 = new(pos2, player2.Score);
                }
            }

            if (rolls == RollLimit)
            {
                movePlayer1 = !movePlayer1;
                rolls = 0;
            }
            else
            {
                rolls++;
            }

            long wins1 = 0;
            long wins2 = 0;

            for (int i = 1; i <= 3; i++)
            {
                (long w1, long w2) = Play((player1, player2, i, rolls, movePlayer1));
                wins1 += w1;
                wins2 += w2;
            }

            return states[state] = (wins1, wins2);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> players = await ReadResourceAsLinesAsync();

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

    private readonly struct Player
    {
        public readonly int Position;
        public readonly int Score;

        public Player(int position, int score = 0)
        {
            Position = position;
            Score = score;
        }

        public readonly void Deconstruct(out int position, out int score)
        {
            position = Position;
            score = Score;
        }
    }
}
