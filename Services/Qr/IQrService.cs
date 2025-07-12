namespace EzUrl.Services.Qr;

public interface IQrService
{
    string GenerateQrCode(string url);
}