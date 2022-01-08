// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 04, "Giant Squid", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the score of the first winning card from playing Bingo.
    /// </summary>
    public int FirstWinningScore { get; private set; }

    /// <summary>
    /// Gets the score of the last winning card from playing Bingo.
    /// </summary>
    public int LastWinningScore { get; private set; }

    /// <summary>
    /// Plays the specified game of Bingo.
    /// </summary>
    /// <param name="game">The lines containing the definition of the game.</param>
    /// <returns>
    /// The score of the first and last winning Bingo cards.
    /// </returns>
    public static (int FirstWinningScore, int LastWinningScore) PlayBingo(IEnumerable<string> game)
    {
        int[] numbers = game
            .First()
            .AsNumbers<int>()
            .ToArray();

        var cards = ParseCards(game);

        int? firstWinningScore = null;
        int? lastWinningScore = null;

        foreach (int number in numbers)
        {
            foreach (BingoCard card in cards)
            {
                if (card.Mark(number) && card.HasWon())
                {
                    int score = card.Score();

                    if (firstWinningScore is null)
                    {
                        firstWinningScore = score;
                    }

                    lastWinningScore = score;
                }
            }
        }

        return (firstWinningScore.GetValueOrDefault(), lastWinningScore.GetValueOrDefault());
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> game = await ReadResourceAsLinesAsync();

        (FirstWinningScore, LastWinningScore) = PlayBingo(game);

        if (Verbose)
        {
            Logger.WriteLine("The score of the first winning Bingo card is {0:N0}.", FirstWinningScore);
            Logger.WriteLine("The score of the last winning Bingo card is {0:N0}.", LastWinningScore);
        }

        return PuzzleResult.Create(FirstWinningScore, LastWinningScore);
    }

    private static List<BingoCard> ParseCards(IEnumerable<string> game)
    {
        var lines = new List<string>();
        var cards = new List<BingoCard>();

        foreach (string line in game.Skip(2))
        {
            if (string.IsNullOrEmpty(line))
            {
                cards.Add(BingoCard.Create(lines));
                lines.Clear();
                continue;
            }

            lines.Add(line);
        }

        cards.Add(BingoCard.Create(lines));

        return cards;
    }

    private sealed class BingoCard
    {
        private readonly Square[,] _squares;
        private int _lastMarked;
        private bool _hasWon;

        private BingoCard(Square[,] squares)
        {
            _squares = squares;
        }

        public static BingoCard Create(IList<string> card)
        {
            var squares = new Square[card.Count, card.Count];

            for (int i = 0; i < card.Count; i++)
            {
                string line = card[i];
                int j = 0;

                foreach (int number in line.AsNumbers<int>(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    squares[i, j++] = new() { Number = number };
                }
            }

            return new(squares);
        }

        public bool HasWon()
        {
            int lengthX = _squares.GetLength(0);
            int lengthY = _squares.GetLength(1);

            for (int x = 0; x < lengthX; x++)
            {
                int rowCount = 0;

                foreach (Square square in _squares.GetRowSpan(x))
                {
                    if (square.IsMarked)
                    {
                        rowCount++;
                    }
                }

                if (rowCount == lengthX)
                {
                    _hasWon = true;
                    return true;
                }
            }

            for (int y = 0; y < lengthY; y++)
            {
                int columnCount = 0;

                foreach (Square square in _squares.GetColumn(y))
                {
                    if (square.IsMarked)
                    {
                        columnCount++;
                    }
                }

                if (columnCount == lengthY)
                {
                    _hasWon = true;
                    return true;
                }
            }

            return false;
        }

        public bool Mark(int number)
        {
            if (_hasWon)
            {
                return false;
            }

            _lastMarked = number;

            int lengthX = _squares.GetLength(0);

            for (int x = 0; x < lengthX; x++)
            {
                foreach (Square square in _squares.GetRowSpan(x))
                {
                    if (square.Number == number)
                    {
                        square.IsMarked = true;
                        return true;
                    }
                }
            }

            return false;
        }

        public int Score()
        {
            int lengthX = _squares.GetLength(0);
            int sum = 0;

            for (int x = 0; x < lengthX; x++)
            {
                foreach (Square square in _squares.GetRowSpan(x))
                {
                    if (!square.IsMarked)
                    {
                        sum += square.Number;
                    }
                }
            }

            return sum * _lastMarked;
        }

        [System.Diagnostics.DebuggerDisplay("{Number} ({IsMarked})")]
        private sealed class Square
        {
            internal int Number { get; init; }

            internal bool IsMarked { get; set; }
        }
    }
}
