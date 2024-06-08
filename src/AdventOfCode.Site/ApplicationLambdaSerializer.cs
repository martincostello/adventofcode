// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.Json;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace MartinCostello.AdventOfCode.Site;

/// <summary>
/// A class representing the JSON serializer for the application to use in AWS Lambda.
/// </summary>
internal sealed class ApplicationLambdaSerializer() : SourceGeneratorLambdaJsonSerializer<ApplicationJsonSerializerContext>()
{
    /// <inheritdoc/>
    protected override JsonSerializerOptions CreateDefaultJsonSerializationOptions()
        => new(ApplicationJsonSerializerContext.Default.Options);
}
