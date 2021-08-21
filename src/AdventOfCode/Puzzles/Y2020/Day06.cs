// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 06, RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the sum of the questions answered by the provided groups by anyone.
    /// </summary>
    public int SumOfQuestionsAnyoneYes { get; private set; }

    /// <summary>
    /// Gets the sum of the questions answered by the provided groups by everyone.
    /// </summary>
    public int SumOfQuestionsEveryoneYes { get; private set; }

    /// <summary>
    /// Gets the sum of the questions answered as yes for the groups and responses
    /// specified by a collection of values.
    /// </summary>
    /// <param name="values">The questions answered yes sorted into groups.</param>
    /// <param name="byEveryone">Whether to return the sum for questions answered by everyone, rather than anyone.</param>
    /// <returns>
    /// The sum of the questions answered yes in each group.
    /// </returns>
    public static int GetSumOfQuestionsWithYesAnswers(ICollection<string> values, bool byEveryone)
    {
        var responses = new List<HashSet<char>>();
        var current = new HashSet<char>();

        int membersOfGroup = 0;

        foreach (string line in values)
        {
            if (string.IsNullOrEmpty(line))
            {
                responses.Add(current);
                current = new HashSet<char>();
                membersOfGroup = 0;
                continue;
            }

            if (!byEveryone || ++membersOfGroup == 1)
            {
                current.UnionWith(line);
            }
            else
            {
                current.IntersectWith(line);
            }
        }

        if (current.Count > 0)
        {
            responses.Add(current);
        }

        return responses.Sum((p) => p.Count);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> values = await ReadResourceAsLinesAsync();

        SumOfQuestionsAnyoneYes = GetSumOfQuestionsWithYesAnswers(values, byEveryone: false);
        SumOfQuestionsEveryoneYes = GetSumOfQuestionsWithYesAnswers(values, byEveryone: true);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the questions answered \"yes\" by anyone in the groups is {0}.", SumOfQuestionsAnyoneYes);
            Logger.WriteLine("The sum of the questions answered \"yes\" by everyone in the groups is {0}.", SumOfQuestionsEveryoneYes);
        }

        return PuzzleResult.Create(SumOfQuestionsAnyoneYes, SumOfQuestionsEveryoneYes);
    }
}
