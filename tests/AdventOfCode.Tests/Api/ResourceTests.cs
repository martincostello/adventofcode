﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Mime;

namespace MartinCostello.AdventOfCode.Api;

public class ResourceTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
    : IntegrationTest(fixture, outputHelper)
{
    [Theory]
    [InlineData("", MediaTypeNames.Text.Html)]
    [InlineData("error.html", MediaTypeNames.Text.Html)]
    [InlineData("favicon.ico", "image/x-icon")]
    [InlineData("humans.txt", MediaTypeNames.Text.Plain)]
    [InlineData("manifest.webmanifest", "application/manifest+json")]
    [InlineData("robots.txt", MediaTypeNames.Text.Plain)]
    [InlineData("robots933456.txt", MediaTypeNames.Text.Plain)]
    [InlineData("static/js/main.js", "text/javascript")]
    [InlineData("static/js/main.js.map", MediaTypeNames.Text.Plain)]
    [InlineData("version", MediaTypeNames.Application.Json)]
    public async Task Can_Load_Resource_As_Get(string requestUri, string contentType)
    {
        // Arrange
        using var client = Fixture.CreateHttpClientForApp();

        // Act
        using var response = await client.GetAsync(requestUri, CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK, await response.Content.ReadAsStringAsync(CancellationToken));
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentType?.MediaType?.ShouldBe(contentType);
    }
}
