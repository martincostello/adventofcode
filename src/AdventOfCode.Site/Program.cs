// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using MartinCostello.AdventOfCode;
using MartinCostello.AdventOfCode.Site;
using MartinCostello.AdventOfCode.Site.Slices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using ILogger = MartinCostello.AdventOfCode.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.CaptureStartupErrors(true);

builder.WebHost.ConfigureKestrel((p) =>
{
    p.AddServerHeader = false;
    p.Limits.MaxRequestBodySize = 200 * 1024; // Maximum upload file size of 200KB
});

builder.Services.AddSingleton<ICache>((_) => NullCache.Instance);
builder.Services.AddSingleton<ILogger, WebLogger>();

builder.Services.AddSingleton<PuzzleFactory>();
builder.Services.AddSingleton(TimeProvider.System);

var puzzles = MartinCostello.AdventOfCode.Site.Program.GetPuzzles();

foreach (var puzzle in puzzles)
{
    builder.Services.AddSingleton(puzzle!);
}

builder.Services.ConfigureHttpJsonOptions((p) =>
{
    p.SerializerOptions.WriteIndented = true;
    p.SerializerOptions.TypeInfoResolverChain.Insert(0, ApplicationJsonSerializerContext.Default);
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

// HACK Disabled until https://github.com/dotnet/aspnetcore/issues/56023 is fixed
if (builder.Environment.IsDevelopment() && RuntimeFeature.IsDynamicCodeSupported)
{
    builder.Services.AddOpenApi((options) =>
    {
        options.AddDocumentTransformer((document, _, _) =>
        {
            document.Info.Title = "Advent of Code";
            document.Info.Version = "v1";

            return Task.CompletedTask;
        });
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

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi, new ApplicationLambdaSerializer());

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

    if (!string.Equals(app.Configuration["ForwardedHeaders_Enabled"], bool.TrueString, StringComparison.OrdinalIgnoreCase))
    {
        app.UseHttpsRedirection();
    }
}

app.UseResponseCompression();

app.UseStaticFiles();

// HACK Disabled until https://github.com/dotnet/aspnetcore/issues/56023 is fixed
if (app.Environment.IsDevelopment() && RuntimeFeature.IsDynamicCodeSupported)
{
    app.MapOpenApi();
}

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
    return new JsonObject()
    {
        ["applicationVersion"] = GitMetadata.Version,
        ["frameworkDescription"] = RuntimeInformation.FrameworkDescription,
        ["operatingSystem"] = new JsonObject()
        {
            ["description"] = RuntimeInformation.OSDescription,
            ["architecture"] = RuntimeInformation.OSArchitecture.ToString(),
            ["version"] = Environment.OSVersion.VersionString,
            ["is64Bit"] = Environment.Is64BitOperatingSystem,
        },
        ["process"] = new JsonObject()
        {
            ["architecture"] = RuntimeInformation.ProcessArchitecture.ToString(),
            ["is64BitProcess"] = Environment.Is64BitProcess,
            ["isNativeAoT"] = !RuntimeFeature.IsDynamicCodeSupported,
            ["isPrivilegedProcess"] = Environment.IsPrivilegedProcess,
        },
        ["dotnetVersions"] = new JsonObject()
        {
            ["runtime"] = GetVersion<object>(),
            ["aspNetCore"] = GetVersion<HttpContext>(),
        },
    };
}).AllowAnonymous()
  .ExcludeFromDescription();

app.MapMethods("/", [HttpMethod.Get.Method, HttpMethod.Head.Method], () => Results.Extensions.RazorSlice<Home>())
   .ExcludeFromDescription();

app.MapMethods("/error", [HttpMethod.Get.Method, HttpMethod.Head.Method, HttpMethod.Post.Method], (HttpContext context) => Results.Extensions.RazorSlice<Error>(context.Response.StatusCode))
   .ExcludeFromDescription()
   .WithMetadata(new ResponseCacheAttribute() { Duration = 0, Location = ResponseCacheLocation.None, NoStore = true });

app.Run();

static string GetVersion<T>()
    => typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

namespace MartinCostello.AdventOfCode.Site
{
#pragma warning disable CA1052
    public partial class Program
#pragma warning restore CA1052
    {
        //// Expose the Program class for use with WebApplicationFactory<T>

        /// <summary>
        /// Gets all the puzzles available in the assembly.
        /// </summary>
        /// <returns>
        /// The attributes for the puzzles which are available to be solved.
        /// </returns>
        [UnconditionalSuppressMessage(
            "Trimming",
            "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
            Justification = "Assembly is trim rooted and all the types requested are available.")]
        internal static List<PuzzleAttribute> GetPuzzles()
        {
            return typeof(Puzzle).Assembly
                .GetTypes()
                .Where((p) => p.IsAssignableTo(typeof(Puzzle)))
                .Select((p) => p.GetCustomAttribute<PuzzleAttribute>())
                .Where((p) => p is not null)
                .Cast<PuzzleAttribute>()
                .Where((p) => !p.IsHidden)
                .Where((p) => !p.Unsolved)
                .ToList();
        }
    }
}
