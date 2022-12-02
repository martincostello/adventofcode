// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 02, "Rock Paper Scissors", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    private enum Move
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    private enum Outcome
    {
        Lose = 0,
        Draw = 3,
        Win = 6,
    }

    /// <summary>
    /// Gets the total score from following the encrypted strategy guide as a series of moves.
    /// </summary>
    public int TotalScoreForMoves { get; private set; }

    /// <summary>
    /// Gets the total score from following the encrypted strategy guide as a series of desired outcomes.
    /// </summary>
    public int TotalScoreForOutcomes { get; private set; }

    /// <summary>
    /// Gets the total score from following the encrypted strategy guide.
    /// </summary>
    /// <param name="moves">The moves to follow.</param>
    /// <param name="containsDesiredOutcome">Whether the guide contains the desired outcomes rather than the moves to play.</param>
    /// <returns>
    /// The total score from following the encrypted strategy guide.
    /// </returns>
    public static int GetTotalScore(ICollection<string> moves, bool containsDesiredOutcome)
    {
        int total = 0;

        Func<string, Move, Move> moveSelector =
            containsDesiredOutcome ?
            (value, opponent) => GetMove(ParseOutcome(value), opponent) :
            (value, _) => ParseMove(value);

        foreach (string move in moves)
        {
            (string opponent, string player) = move.AsPair(' ');

            Move opponentMove = ParseMove(opponent);
            Move playerMove = moveSelector(player, opponentMove);

            var outcome = GetOutcome(playerMove, opponentMove);

            total += (int)playerMove + (int)outcome;
        }

        return total;

        static Move GetMove(Outcome outcome, Move opponent) => (outcome, opponent) switch
        {
            (Outcome.Draw, _) => opponent,
            (Outcome.Win, Move.Rock) => Move.Paper,
            (Outcome.Win, Move.Paper) => Move.Scissors,
            (Outcome.Win, Move.Scissors) => Move.Rock,
            (Outcome.Lose, Move.Rock) => Move.Scissors,
            (Outcome.Lose, Move.Paper) => Move.Rock,
            (Outcome.Lose, Move.Scissors) => Move.Paper,
            _ => throw new InvalidOperationException("Invalid outcome and move combination."),
        };

        static Outcome GetOutcome(Move player, Move opponent) => (player, opponent) switch
        {
            (Move.Rock, Move.Paper) => Outcome.Lose,
            (Move.Rock, Move.Scissors) => Outcome.Win,
            (Move.Paper, Move.Rock) => Outcome.Win,
            (Move.Paper, Move.Scissors) => Outcome.Lose,
            (Move.Scissors, Move.Rock) => Outcome.Lose,
            (Move.Scissors, Move.Paper) => Outcome.Win,
            _ => player == opponent ? Outcome.Draw : throw new InvalidOperationException("Invalid move combination."),
        };

        static Move ParseMove(string value) => value switch
        {
            "A" or "X" => Move.Rock,
            "B" or "Y" => Move.Paper,
            "C" or "Z" => Move.Scissors,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid move."),
        };

        static Outcome ParseOutcome(string value) => value switch
        {
            "X" => Outcome.Lose,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid outcome."),
        };
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var moves = await ReadResourceAsLinesAsync();

        TotalScoreForMoves = GetTotalScore(moves, containsDesiredOutcome: false);
        TotalScoreForOutcomes = GetTotalScore(moves, containsDesiredOutcome: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The total score from following the encrypted strategy guide of moves would be {0:N0}.",
                TotalScoreForMoves);

            Logger.WriteLine(
                "The total score from following the encrypted strategy guide of desired outcomes would be {0:N0}.",
                TotalScoreForOutcomes);
        }

        return PuzzleResult.Create(TotalScoreForMoves, TotalScoreForOutcomes);
    }
}
