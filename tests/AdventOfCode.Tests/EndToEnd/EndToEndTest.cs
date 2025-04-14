// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.EndToEnd;

[Category("EndToEnd")]
[Collection<SiteCollection>]
public abstract class EndToEndTest(SiteFixture fixture)
{
    protected virtual CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    protected SiteFixture Fixture { get; } = fixture;
}
