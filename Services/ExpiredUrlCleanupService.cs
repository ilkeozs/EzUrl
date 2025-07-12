using EzUrl.Data;
using Microsoft.EntityFrameworkCore;

namespace EzUrl.Services;

public class ExpiredUrlCleanupService : BackgroundService
{
    #region Fields
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredUrlCleanupService> _logger;
    #endregion

    #region Constructor
    public ExpiredUrlCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredUrlCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    #endregion

    #region Methods
    private async Task CleanupExpiredUrls()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Find and delete expired urls
        var expiredUrls = await context.ShortUrls
            .Where(x => x.ExpiresAt < DateTime.Now)
            .ToListAsync();

        if (expiredUrls.Any())
        {
            context.ShortUrls.RemoveRange(expiredUrls);
            await context.SaveChangesAsync();
            _logger.LogInformation("{Count} expired urls deleted.", expiredUrls.Count);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredUrls();
                // Check every 30 minutes
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up expired urls.");
                // Wait 5 minutes if there is an error
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
    #endregion
}