// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 06, "Signals and Noise", RequiresData = true)]
public sealed class Day06 : Puzzle<string, string>
{
    /// <summary>
    /// Decrypts the specified message using a repetition code.
    /// </summary>
    /// <param name="messages">The jammed messages.</param>
    /// <param name="leastLikely">Whether to use the least likely value instead of the most.</param>
    /// <returns>
    /// The decrypted message derived from <paramref name="messages"/>.
    /// </returns>
    internal static string DecryptMessage(IList<string> messages, bool leastLikely)
    {
        int length = messages[0].Length;

        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            var characters = messages
                .Select((p) => p[i])
                .CountBy((p) => p)
                .OrderBy((p) => p.Value)
                .Select((p) => p.Key);

            char ch =
                leastLikely ?
                characters.First() :
                characters.Last();

            result.Append(ch);
        }

        return result.ToString();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var messages = await ReadResourceAsLinesAsync(cancellationToken);

        Solution1 = DecryptMessage(messages, leastLikely: false);
        Solution2 = DecryptMessage(messages, leastLikely: true);

        if (Verbose)
        {
            Logger.WriteLine($"The error-corrected message using the most likely letters is: {Solution1}.");
            Logger.WriteLine($"The error-corrected message using the least likely letters is: {Solution2}.");
        }

        return Result();
    }
}
