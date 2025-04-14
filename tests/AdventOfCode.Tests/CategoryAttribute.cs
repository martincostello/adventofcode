// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Xunit.v3;

namespace MartinCostello.AdventOfCode;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class CategoryAttribute(string category) : Attribute, ITraitAttribute
{
    public string Category { get; } = category;

    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        => [new("Category", Category)];
}
