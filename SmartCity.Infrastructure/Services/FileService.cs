using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SmartCity.Application.Interfaces;

namespace SmartCity.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only JPG and PNG images are allowed");

            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("File size exceeds 5MB");

            // 🔥 FIX: Handle null WebRootPath
            var rootPath = _environment.WebRootPath;

            if (string.IsNullOrEmpty(rootPath))
            {
                rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolder = Path.Combine(rootPath, "uploads", "issues");

            // 🔥 Ensure folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 🔥 Return relative path (for API usage)
            return $"uploads/issues/{fileName}";
        }
    }
}