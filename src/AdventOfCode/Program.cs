// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using MartinCostello.AdventOfCode;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using ILogger = MartinCostello.AdventOfCode.ILogger;

if (args.FirstOrDefault() == "--solve")
{
    return await RunSolverAsync(args[1..], new ConsoleLogger());
}

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Advent of Code", Version = "v1" });
});

builder.Services.Configure<GzipCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

builder.Services.Configure<BrotliCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

builder.Services.AddResponseCompression((p) =>
{
    p.EnableForHttps = true;
    p.Providers.Add<BrotliCompressionProvider>();
    p.Providers.Add<GzipCompressionProvider>();
});

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

app.UseSwagger();

app.MapRazorPages();

app.MapGet("/api/puzzles", PuzzlesApi.GetPuzzlesAsync);

app.MapPost("/api/puzzles/{year:int}/{day:int}/solve", PuzzlesApi.SolvePuzzleAsync);

if (app.Environment.IsDevelopment())
{
    app.MapGet("/api", () => Results.Redirect("/swagger-ui/index.html"))
       .ExcludeFromDescription();
}

await app.RunAsync();

return 0;

static async Task<int> RunSolverAsync(string[] args, ILogger logger)
{
    if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int day))
    {
        day = 0;
    }

    int year = 0;

    if (args.Length > 1)
    {
        if (!int.TryParse(args[1], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out year))
        {
            year = 0;
        }

        args = args[2..];
    }
    else
    {
        year = DateTime.UtcNow.Year;
        args = args[1..];
    }

    using var cts = new CancellationTokenSource();

    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        cts.Cancel();
    };

    return await SolvePuzzleAsync(year, day, args, logger, cts.Token);
}

static async Task<int> SolvePuzzleAsync(
    int year,
    int day,
    string[] args,
    ILogger logger,
    CancellationToken cancellationToken)
{
    var factory = new PuzzleFactory(logger);

    Puzzle puzzle;

    try
    {
        puzzle = factory.Create(year, day);
    }
    catch (PuzzleException ex)
    {
        logger.WriteLine(ex.Message);
        return -1;
    }

    logger.WriteLine();
    logger.WriteLine($"Advent of Code {year} - Day {day}");
    logger.WriteLine();

    var stopwatch = Stopwatch.StartNew();

    try
    {
        _ = await puzzle.SolveAsync(args, cancellationToken);
    }
    catch (OperationCanceledException)
    {
        logger.WriteLine("Solution canceled.");
        return -1;
    }
    catch (PuzzleException ex)
    {
        logger.WriteLine(ex.Message);
        return -1;
    }

    stopwatch.Stop();

    logger.WriteLine();

    if (stopwatch.Elapsed.TotalSeconds < 0.01f)
    {
        logger.WriteLine("Took <0.01 seconds.");
    }
    else
    {
        logger.WriteLine($"Took {stopwatch.Elapsed.TotalSeconds:N2} seconds.");
    }

    logger.WriteLine();

    return 0;
}
