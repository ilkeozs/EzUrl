
using EzUrl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzUrl.Data.Configurations;

public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        var seedDate = new DateTime(2025, 7, 12);

        builder.HasData(new ShortUrl
        {
            Id = 1,
            Url = "https://www.google.com",
            ShortCode = "12345",
            CreatedAt = seedDate,
            ExpiresAt = seedDate.AddHours(24),
            QrCodeBase64 = null
        });
    }
}