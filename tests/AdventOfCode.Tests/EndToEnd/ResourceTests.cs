﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.EndToEnd
{
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;

    public class ResourceTests : EndToEndTest
    {
        public ResourceTests(SiteFixture fixture)
            : base(fixture)
        {
        }

        [SkippableTheory]
        [InlineData("/", MediaTypeNames.Text.Html)]
        [InlineData("/error.html", MediaTypeNames.Text.Html)]
        [InlineData("/favicon.ico", "image/x-icon")]
        [InlineData("/humans.txt", MediaTypeNames.Text.Plain)]
        [InlineData("/manifest.webmanifest", "application/manifest+json")]
        [InlineData("/robots.txt", MediaTypeNames.Text.Plain)]
        [InlineData("/static/js/main.js", "application/javascript")]
        [InlineData("/static/js/main.js.map", MediaTypeNames.Text.Plain)]
        public async Task Can_Load_Resource_As_Get(string requestUri, string contentType)
        {
            // Arrange
            using var client = Fixture.CreateClient();

            // Act
            using var response = await client.GetAsync(requestUri);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
            response.Content.ShouldNotBeNull();
            response.Content!.Headers.ContentType?.MediaType?.ShouldBe(contentType);
        }
    }
}
