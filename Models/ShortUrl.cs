using System.ComponentModel.DataAnnotations;

namespace EzUrl.Models;

public class ShortUrl : BaseClass
{
    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Please enter a valid URL.")]
    [MaxLength(1024, ErrorMessage = "URL cannot exceed 1024 characters.")]
    public string Url { get; set; } = string.Empty;
    
    [MaxLength(5, ErrorMessage = "Short code cannot exceed 5 characters.")]
    public string ShortCode { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public string? QrCodeBase64 { get; set; }
    
    public ShortUrl()
    {
        CreatedAt = DateTime.Now;
        ExpiresAt = CreatedAt.AddHours(24);
    }
}