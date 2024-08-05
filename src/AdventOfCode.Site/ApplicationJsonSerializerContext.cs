// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;

namespace MartinCostello.AdventOfCode.Site;

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(uint))]
[JsonSerializable(typeof(ulong))]
[JsonSerializable(typeof(APIGatewayProxyRequest))]
[JsonSerializable(typeof(APIGatewayProxyResponse))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(List<PuzzlesApi.PuzzleMetadata>))]
[JsonSerializable(typeof(PuzzlesApi.PuzzleSolution))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class ApplicationJsonSerializerContext : JsonSerializerContext;
