// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MartinCostello.AdventOfCode;

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(uint))]
[JsonSerializable(typeof(ulong))]
[JsonSerializable(typeof(List<PuzzlesApi.PuzzleMetadata>))]
[JsonSerializable(typeof(PuzzlesApi.PuzzleSolution))]
internal sealed partial class ApplicationJsonSerializerContext : JsonSerializerContext
{
}
