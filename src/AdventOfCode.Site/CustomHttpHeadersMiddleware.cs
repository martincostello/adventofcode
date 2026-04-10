// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography;
using Microsoft.Net.Http.Headers;

namespace MartinCostello.AdventOfCode.Site;

/// <summary>
/// A class representing a middleware for writing custom HTTP headers.
/// </summary>
/// <param name="next">The next request delegate in the pipeline.</param>
public sealed class CustomHttpHeadersMiddleware(RequestDelegate next)
{
    private static readonly string BaseContentSecurityPolicy = string.Join(
        ';',
        "default-src 'self'",
        "img-src 'self' data:",
        "font-src 'self' cdnjs.cloudflare.com",
        "connect-src 'self' cdnjs.cloudflare.com",
        "media-src 'none'",
        "object-src 'none'",
        "child-src 'self'",
        "frame-ancestors 'none'",
        "form-action 'self'",
        "block-all-mixed-content",
        "base-uri 'self'",
        "manifest-src 'self'",
        "upgrade-insecure-requests");

    /// <summary>
    /// Invokes the specified middleware.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="environment">The current web host environment.</param>
    /// <returns>
    /// The result of invoking the middleware.
    /// </returns>
    public Task Invoke(HttpContext context, IWebHostEnvironment environment)
    {
        byte[] data = RandomNumberGenerator.GetBytes(32);
        string nonce = System.Buffers.Text.Base64Url.EncodeToString(data);

        context.Items["csp-hash"] = nonce;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Remove(HeaderNames.Server);
            context.Response.Headers.Remove(HeaderNames.XPoweredBy);

            if (environment.IsProduction())
            {
                context.Response.Headers.ContentSecurityPolicy = ContentSecurityPolicy(nonce);
            }

            context.Response.Headers.Append("Cross-Origin-Embedder-Policy", "unsafe-none");
            context.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin");
            context.Response.Headers.Append("Cross-Origin-Resource-Policy", "same-origin");

            if (context.Request.IsHttps)
            {
                context.Response.Headers.Append("Expect-CT", "max-age=1800");
            }

            context.Response.Headers.Append("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            context.Response.Headers.Append("Referrer-Policy", "no-referrer-when-downgrade");
            context.Response.Headers.XContentTypeOptions = "nosniff";
            context.Response.Headers.Append("X-Download-Options", "noopen");

            if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
            {
                context.Response.Headers.XFrameOptions = "DENY";
            }

            context.Response.Headers.Append("X-Request-Id", context.TraceIdentifier);
            context.Response.Headers.XXSSProtection = "1; mode=block";

            return Task.CompletedTask;
        });

        return next(context);
    }

    private static string ContentSecurityPolicy(string nonce)
    {
        var builder = new StringBuilder(BaseContentSecurityPolicy);

        string directive = $" 'self' 'nonce-{nonce}' cdnjs.cloudflare.com;";

        builder.Append(';')
               .Append("script-src")
               .Append(directive)
               .Append("script-src-elem")
               .Append(directive)
               .Append("style-src")
               .Append(directive)
               .Append("style-src-elem")
               .Append(directive);

        return builder.ToString();
    }
}
