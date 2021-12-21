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

        var winStates = new Dictionary<(Player P1, Player P2), (long Wins1, long Wins2)>();

        // The frequency distribution for each possiblity of throwing three three-sided dice (3d3).
        // See https://www.wolframalpha.com/input/?i2d=true&i=3d3 for the actual distribution.
        (int Roll, int Frequency)[] rolls3D3 =
        {
            new(3, 1),
            new(4, 3),
            new(5, 6),
            new(6, 7),
            new(7, 6),
            new(8, 3),
            new(9, 1),
        };

        (long wins1, long wins2) = Play(player1, player2);

        return Math.Max(wins1, wins2);

        (long Wins1, long Wins2) Play(Player player1, Player player2)
        {
            const int WinningScore = 21;

            if (player2.Score >= WinningScore)
            {
                return (0, 1);
            }
            else if (winStates.TryGetValue((player1, player2), out var wins))
            {
                // We already know the result for this game state
                return wins;
            }

            long wins1 = 0;
            long wins2 = 0;

            foreach ((int roll, int frequency) in rolls3D3)
            {
                Player next = Move(player1, roll);

                (long next2, long next1) = Play(player2, next);

                wins1 += next1 * frequency;
                wins2 += next2 * frequency;
            }

            return (wins1, wins2);
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
        int position = ((player.Position - 1 + roll) % 10) + 1;
        return new(position, player.Score + position);
    }

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
