// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.IO.Compression;
using System.Reflection;
using MartinCostello.AdventOfCode;
using MartinCostello.AdventOfCode.Site;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using ILogger = MartinCostello.AdventOfCode.ILogger;

#pragma warning disable CA1812

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.CaptureStartupErrors(true);

builder.WebHost.ConfigureKestrel((p) => p.AddServerHeader = false);

builder.Services.AddSingleton<ILogger, WebLogger>();

builder.Services.AddSingleton<PuzzleFactory>();

var puzzles = typeof(Puzzle).Assembly
    .GetTypes()
    .Where((p) => p.IsAssignableTo(typeof(Puzzle)))
    .Select((p) => p.GetCustomAttribute<PuzzleAttribute>())
    .Where((p) => p is not null)
    .Where((p) => !p!.IsHidden)
    .ToList();

foreach (var puzzle in puzzles)
{
    builder.Services.AddSingleton(puzzle!);
}

builder.Services.Configure<JsonOptions>((p) =>
{
    p.SerializerOptions.WriteIndented = true;
    p.SerializerOptions.AddContext<ApplicationJsonSerializerContext>();
});

builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new() { Title = "Advent of Code", Version = "v1" });
    });
}

builder.Services.Configure<GzipCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

builder.Services.Configure<BrotliCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

builder.Services.AddResponseCompression((p) =>
{
    p.EnableForHttps = true;
    p.Providers.Add<BrotliCompressionProvider>();
    p.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?id={0}");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseResponseCompression();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.MapRazorPages();

app.MapGet("/api/puzzles", PuzzlesApi.GetPuzzlesAsync);

app.MapPost("/api/puzzles/{year:int}/{day:int}/solve", PuzzlesApi.SolvePuzzleAsync);

if (app.Environment.IsDevelopment())
{
    app.MapGet("/api", () => Results.Redirect("/swagger-ui/index.html"))
       .ExcludeFromDescription();
}

app.Run();

namespace MartinCostello.AdventOfCode.Site
{
    public partial class Program
    {
        // Expose the Program class for use with WebApplicationFactory<T>
    }
}
