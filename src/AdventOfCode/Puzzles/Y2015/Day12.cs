// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.Json;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 12, RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// Gets the sum of the integers in the JSON document.
    /// </summary>
    internal long Sum { get; private set; }

    /// <summary>
    /// Sums the integer values in the specified JSON element, ignoring any values from
    /// child elements that contain the specified string value, if specified.
    /// </summary>
    /// <param name="element">The JSON element.</param>
    /// <param name="valueToIgnore">The elements to ignore if they contain this value.</param>
    /// <returns>The sum of the elements in <paramref name="element"/>.</returns>
    internal static long SumIntegerValues(JsonElement element, string valueToIgnore)
    {
        long sum = 0;

        if (element.ValueKind == JsonValueKind.Number)
        {
            sum = element.GetInt64();
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement child in element.EnumerateArray())
            {
                sum += SumIntegerValues(child, valueToIgnore);
            }
        }
        else if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty child in element.EnumerateObject())
            {
                if (child.Value.ValueKind == JsonValueKind.String &&
                    child.Value.GetString() == valueToIgnore)
                {
                    return 0;
                }

                sum += SumIntegerValues(child.Value, valueToIgnore);
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        Stream resource = Resource ?? ReadResource();

        try
        {
            using var document = await JsonDocument.ParseAsync(resource, cancellationToken: cancellationToken);
            string keyToIgnore = args.Length > 0 ? args[0] : string.Empty;

            Sum = SumIntegerValues(document.RootElement, keyToIgnore);

            if (Verbose)
            {
                Logger.WriteLine("The sum of the integers in the JSON document is {0:N0}.", Sum);
            }
        }
        finally
        {
            if (Resource is null)
            {
                await resource.DisposeAsync();
            }
        }

        return PuzzleResult.Create(Sum);
    }
}
