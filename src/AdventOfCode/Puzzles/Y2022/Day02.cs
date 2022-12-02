// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 02, "Rock Paper Scissors", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    private static readonly Dictionary<string, Move> Moves = new(StringComparer.Ordinal)
    {
        ["A"] = Move.Rock,
        ["B"] = Move.Paper,
        ["C"] = Move.Scissors,
        ["X"] = Move.Rock,
        ["Y"] = Move.Paper,
        ["Z"] = Move.Scissors,
    };

    private static readonly Dictionary<(Move Player, Move Opponent), Outcome> Outcomes = new()
    {
        [(Move.Rock, Move.Rock)] = Outcome.Draw,
        [(Move.Rock, Move.Paper)] = Outcome.Lose,
        [(Move.Rock, Move.Scissors)] = Outcome.Win,
        [(Move.Paper, Move.Rock)] = Outcome.Win,
        [(Move.Paper, Move.Paper)] = Outcome.Draw,
        [(Move.Paper, Move.Scissors)] = Outcome.Lose,
        [(Move.Scissors, Move.Rock)] = Outcome.Lose,
        [(Move.Scissors, Move.Paper)] = Outcome.Win,
        [(Move.Scissors, Move.Scissors)] = Outcome.Draw,
    };

    private enum Move
    {
        Rock,
        Paper,
        Scissors,
    }

    private enum Outcome
    {
        Lose,
        Draw,
        Win,
    }

    /// <summary>
    /// Gets the total score from following the encrypted strategy guide.
    /// </summary>
    public int TotalScore { get; private set; }

    /// <summary>
    /// Gets the total score from following the encrypted strategy guide.
    /// </summary>
    /// <param name="moves">The moves to follow.</param>
    /// <returns>
    /// The total score from following the encrypted strategy guide.
    /// </returns>
    public static int GetTotalScore(ICollection<string> moves)
    {
        int total = 0;

        foreach (string move in moves)
        {
            (string opponent, string player) = move.AsPair(' ');

            var playerMove = Moves[player];
            var opponentMove = Moves[opponent];
            var outcome = Outcomes[(playerMove, opponentMove)];

            total += GetScore(playerMove, outcome);
        }

        return total;

        static int GetScore(Move player, Outcome outcome)
        {
            int scoreForShape = player switch
            {
                Move.Rock => 1,
                Move.Paper => 2,
                Move.Scissors => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(player), player, "Invalid move."),
            };

            int scoreForOutcome = outcome switch
            {
                Outcome.Lose => 0,
                Outcome.Draw => 3,
                Outcome.Win => 6,
                _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Invalid outcome."),
            };

            return scoreForShape + scoreForOutcome;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var moves = await ReadResourceAsLinesAsync();

        TotalScore = GetTotalScore(moves);

        if (Verbose)
        {
            Logger.WriteLine(
                "The total score from following the encrypted strategy guide would be {0:N0}.",
                TotalScore);
        }

        return PuzzleResult.Create(TotalScore);
    }
}
