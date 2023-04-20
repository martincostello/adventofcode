// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Microsoft.Playwright;

namespace MartinCostello.AdventOfCode;

public class BrowserFixture
{
    private const string VideosDirectory = "videos";
    private static readonly string AssetsDirectory = Path.Combine("..", "..", "..");

    public BrowserFixture(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    private static bool IsRunningInGitHubActions { get; } = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"));

    private ITestOutputHelper OutputHelper { get; }

    public async Task WithPageAsync(
        Func<IPage, Task> action,
        string browserType = "chromium",
        [CallerMemberName] string? testName = null)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await CreateBrowserAsync(playwright, browserType);

        var options = CreateContextOptions();
        await using var context = await browser.NewContextAsync(options);

        if (IsRunningInGitHubActions)
        {
            await context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
                Title = testName,
            });
        }

        var page = await context.NewPageAsync();

        page.Console += (_, e) => OutputHelper.WriteLine(e.Text);
        page.PageError += (_, e) => OutputHelper.WriteLine(e);

        try
        {
            await action(page);
        }
        catch (Exception)
        {
            if (IsRunningInGitHubActions)
            {
                string traceName = GenerateFileName(testName!, browserType, ".zip");
                string path = Path.Combine(AssetsDirectory, "traces", traceName);

                await context.Tracing.StopAsync(new() { Path = path });

                OutputHelper.WriteLine($"Trace saved to {path}.");
            }

            await TryCaptureScreenshotAsync(page, testName!, browserType);
            throw;
        }
        finally
        {
            await TryCaptureVideoAsync(page, testName!, browserType);
        }
    }

    protected virtual BrowserNewContextOptions CreateContextOptions()
    {
        var options = new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true,
            Locale = "en-GB",
            TimezoneId = "Europe/London",
        };

        if (IsRunningInGitHubActions)
        {
            options.RecordVideoDir = VideosDirectory;
        }

        return options;
    }

    private static async Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, string browserType)
    {
        var options = new BrowserTypeLaunchOptions();

        if (System.Diagnostics.Debugger.IsAttached)
        {
            options.Devtools = true;
            options.Headless = false;
            options.SlowMo = 100;
        }

        string[] split = browserType.Split(':');

        browserType = split[0];

        if (split.Length > 1)
        {
            options.Channel = split[1];
        }

        return await playwright[browserType].LaunchAsync(options);
    }

    private static string GenerateFileName(string testName, string browserType, string extension)
    {
        string os =
            OperatingSystem.IsLinux() ? "linux" :
            OperatingSystem.IsMacOS() ? "macos" :
            OperatingSystem.IsWindows() ? "windows" :
            "other";

        browserType = browserType.Replace(':', '_');

        string utcNow = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
        return $"{testName}_{browserType}_{os}_{utcNow}{extension}";
    }

    private async Task TryCaptureScreenshotAsync(
        IPage page,
        string testName,
        string browserType)
    {
        try
        {
            string fileName = GenerateFileName(testName, browserType, ".png");
            string path = Path.GetFullPath(Path.Combine(AssetsDirectory, "screenshots", fileName));

            await page.ScreenshotAsync(new() { Path = path });

            OutputHelper.WriteLine($"Screenshot saved to {path}.");
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            OutputHelper.WriteLine("Failed to capture screenshot: " + ex);
        }
    }

    private async Task TryCaptureVideoAsync(
        IPage page,
        string testName,
        string browserType)
    {
        if (!IsRunningInGitHubActions || page.Video is null)
        {
            return;
        }

        try
        {
            await page.CloseAsync();

            string videoSource = await page.Video.PathAsync();

            string? directory = Path.GetDirectoryName(videoSource);
            string? extension = Path.GetExtension(videoSource);

            string fileName = GenerateFileName(testName, browserType, extension!);

            string videoDestination = Path.Combine(AssetsDirectory, directory!, fileName);

            File.Move(videoSource, videoDestination);

            OutputHelper.WriteLine($"Video saved to {videoDestination}.");
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            OutputHelper.WriteLine("Failed to capture video: " + ex);
        }
    }
}
