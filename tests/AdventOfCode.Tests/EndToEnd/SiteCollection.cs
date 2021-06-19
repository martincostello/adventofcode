// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.EndToEnd
{
    using Xunit;

    [CollectionDefinition(Name)]
    public sealed class SiteCollection : ICollectionFixture<SiteFixture>
    {
        public const string Name = "Site collection";
    }
}
