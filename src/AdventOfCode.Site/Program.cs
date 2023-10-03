// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using MartinCostello.AdventOfCode;
using MartinCostello.AdventOfCode.Site;
using Microsoft.AspNetCore.ResponseCompression;
using ILogger = MartinCostello.AdventOfCode.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.CaptureStartupErrors(true);

builder.WebHost.ConfigureKestrel((p) =>
{
    p.AddServerHeader = false;
    p.Limits.MaxRequestBodySize = 10 * 1024; // Maximum upload file size of 10KB
});

builder.Services.AddSingleton<ICache>((_) => NullCache.Instance);
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

builder.Services.ConfigureHttpJsonOptions((p) =>
{
    p.SerializerOptions.WriteIndented = true;
    p.SerializerOptions.TypeInfoResolverChain.Add(ApplicationJsonSerializerContext.Default);
});

builder.Services.Configure<StaticFileOptions>((options) =>
{
    options.OnPrepareResponse = (context) =>
    {
        var maxAge = TimeSpan.FromDays(7);

        if (context.File.Exists)
        {
            string? extension = Path.GetExtension(context.File.PhysicalPath);

            // These files are served with a content hash in the URL so can be cached for longer
            bool isScriptOrStyle =
                string.Equals(extension, ".css", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".js", StringComparison.OrdinalIgnoreCase);

            if (isScriptOrStyle)
            {
                maxAge = TimeSpan.FromDays(365);
            }
        }

        var headers = context.Context.Response.GetTypedHeaders();
        headers.CacheControl = new() { MaxAge = maxAge };
    };
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

builder.Services.Configure<BrotliCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

builder.Services.AddResponseCompression((p) =>
{
    p.EnableForHttps = true;
    p.Providers.Add<BrotliCompressionProvider>();
    p.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseMiddleware<CustomHttpHeadersMiddleware>();

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

app.MapPost("/api/puzzles/{year:int}/{day:int}/solve", PuzzlesApi.SolvePuzzleAsync)
   .DisableAntiforgery();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/api", () => Results.Redirect("/swagger-ui/index.html"))
       .ExcludeFromDescription();
}

app.MapGet("/version", () =>
{
    var result = new
    {
        applicationVersion = GetVersion<ApplicationJsonSerializerContext>(),
        frameworkDescription = RuntimeInformation.FrameworkDescription,
        operatingSystem = RuntimeInformation.OSDescription,
        operatingSystemArchitecture = RuntimeInformation.OSArchitecture.ToString(),
        processArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
        dotnet = new
        {
            runtime = GetVersion<object>(),
            aspNetCore = GetVersion<RequestDelegateFactoryOptions>(),
        },
    };

    return Results.Json(result);
}).AllowAnonymous();

app.Run();

static string GetVersion<T>()
    => typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

namespace MartinCostello.AdventOfCode.Site
{
    public partial class Program
    {
        // Expose the Program class for use with WebApplicationFactory<T>
    }
}
