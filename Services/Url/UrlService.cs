using EzUrl.Data;
using EzUrl.Models;
using EzUrl.Services.Qr;
using Microsoft.EntityFrameworkCore;

namespace EzUrl.Services;

public class UrlService : IUrlService
{
    #region Fields
    private readonly ApplicationDbContext _context;
    private readonly IQrService _qrService;
    private readonly IConfiguration _configuration;
    #endregion

    #region Constructors
    public UrlService(ApplicationDbContext context, IQrService qrService, IConfiguration configuration)
    {
        _context = context;
        _qrService = qrService;
        _configuration = configuration;
    }
    #endregion

    #region Methods
    public string GenerateRandomCode()
    {
        return Guid.NewGuid().ToString("N")[..5].ToUpper();
    }

    public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl, string? baseUrl = null)
    {
        var shortCode = GenerateRandomCode();
        
        // BaseUrl priority: parameter > configuration > fallback
        var finalBaseUrl = baseUrl;
        
        var fullShortUrl = $"{finalBaseUrl}/{shortCode}";
        
        // Generate QR code
        var qrCodeBase64 = _qrService.GenerateQrCode(fullShortUrl);
        
        var shortUrl = new ShortUrl
        {
            Url = originalUrl,
            ShortCode = shortCode,
            QrCodeBase64 = qrCodeBase64
        };

        _context.ShortUrls.Add(shortUrl);
        await _context.SaveChangesAsync();
        return shortUrl;
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.ShortUrls
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
    }
    #endregion
}