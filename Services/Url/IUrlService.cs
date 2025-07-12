using EzUrl.Models;

namespace EzUrl.Services;

public interface IUrlService
{
    string GenerateRandomCode();
    Task<ShortUrl> CreateShortUrlAsync(string originalUrl, string? baseUrl = null);
    Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
}