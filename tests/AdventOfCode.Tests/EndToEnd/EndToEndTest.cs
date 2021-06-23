// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.EndToEnd
{
    [Collection(SiteCollection.Name)]
    [Trait("Category", "EndToEnd")]
    public abstract class EndToEndTest
    {
        protected EndToEndTest(SiteFixture fixture)
        {
            Fixture = fixture;
        }

        protected SiteFixture Fixture { get; }
    }
}
