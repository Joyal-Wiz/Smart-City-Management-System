using Microsoft.AspNetCore.Http;

namespace SmartCity.Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file);
    }
}