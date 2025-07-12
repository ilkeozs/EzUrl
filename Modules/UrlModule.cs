using Carter;
using EzUrl.Data;
using EzUrl.Models;
using Microsoft.EntityFrameworkCore;
using EzUrl.Services;

namespace EzUrl;

public class UrlModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Redirect endpoint for short urls
        app.MapGet("/{shortCode}", async (IUrlService urlService, string shortCode) =>
        {
            var shortUrl = await urlService.GetByShortCodeAsync(shortCode);
            
            if (shortUrl is null)
            {
                return Results.NotFound("URL not found.");
            }

            return Results.Redirect(shortUrl.Url);
        }).ExcludeFromDescription(); // This endpoint is not included in the Scalar documentation


        // API group for urls
        var group = app.MapGroup("/api/urls");

        // Get all urls
        group.MapGet(string.Empty, async (ApplicationDbContext context) =>
        {
            return Results.Ok(await context.ShortUrls.ToListAsync());
        }).Produces<List<ShortUrl>>();

        // Get a url by id
        group.MapGet("{id}", async (ApplicationDbContext context, int id) =>
        {
            return Results.Ok(await context.ShortUrls.FindAsync(id));
        }).Produces<ShortUrl>();

        // Create a new url
        group.MapPost(string.Empty, async (IUrlService urlService, ShortUrl shortUrl, HttpContext httpContext) =>
        {
            // Get the base url from the http context
            var scheme = httpContext.Request.Scheme; // http or https
            var host = httpContext.Request.Host.Value; // host:port
            var baseUrl = $"{scheme}://{host}";
            
            var result = await urlService.CreateShortUrlAsync(shortUrl.Url, baseUrl);
            return Results.Ok(result);
        }).Produces<ShortUrl>();

        // Get QR code for a short URL
        group.MapGet("{shortCode}/qr", async (IUrlService urlService, string shortCode) =>
        {
            var shortUrl = await urlService.GetByShortCodeAsync(shortCode);
            
            if (shortUrl is null)
            {
                return Results.NotFound("URL not found.");
            }

            if (string.IsNullOrEmpty(shortUrl.QrCodeBase64))
            {
                return Results.NotFound("QR code not found.");
            }

            var qrCodeBytes = Convert.FromBase64String(shortUrl.QrCodeBase64);
            return Results.File(qrCodeBytes, "image/png", $"qr-{shortCode}.png");
        }).Produces(200, typeof(IResult), "image/png");
    }
}