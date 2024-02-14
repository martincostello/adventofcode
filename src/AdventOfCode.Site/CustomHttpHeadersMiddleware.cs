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
    private static readonly CompositeFormat ContentSecurityPolicyTemplate = CompositeFormat.Parse(string.Join(
        ';',
        [
            "default-src 'self'",
            "script-src 'self' 'nonce-{0}' cdnjs.cloudflare.com",
            "script-src-elem 'self' 'nonce-{0}' cdnjs.cloudflare.com",
            "style-src 'self' 'nonce-{0}' cdnjs.cloudflare.com",
            "style-src-elem 'self' 'nonce-{0}' cdnjs.cloudflare.com",
            "img-src 'self' data:",
            "font-src 'self' cdnjs.cloudflare.com",
            "connect-src 'self'",
            "media-src 'none'",
            "object-src 'none'",
            "child-src 'self'",
            "frame-ancestors 'none'",
            "form-action 'self'",
            "block-all-mixed-content",
            "base-uri 'self'",
            "manifest-src 'self'",
            "upgrade-insecure-requests",
        ]));

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
        string nonce = Convert.ToBase64String(data).Replace('+', '/');

        context.Items["csp-hash"] = nonce;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Remove(HeaderNames.Server);
            context.Response.Headers.Remove(HeaderNames.XPoweredBy);

            if (environment.IsProduction())
            {
                context.Response.Headers.ContentSecurityPolicy = ContentSecurityPolicy(nonce);
            }

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
        => string.Format(CultureInfo.InvariantCulture, ContentSecurityPolicyTemplate, nonce);
}
