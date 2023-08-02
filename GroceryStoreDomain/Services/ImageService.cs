using GroceryStoreDomain.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GroceryStoreDomain.Services
{
    public class ImageService : IImageService
    {
        public string GetFilePath(string contentRootPath, IFormFile ImageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception("Invalid file type. Only JPG, JPEG and PNG files are allowed.");

            var uploadsFolder = Path.Combine(contentRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            return filePath;
        }

        public byte[] GetFileBytes(string imageFilePath)
        {
            var imageBytes = File.ReadAllBytes(imageFilePath);
            return imageBytes;
        }

        public string GetImageContentType(string imageFilePath)
        {
            var extension = Path.GetExtension(imageFilePath).ToLowerInvariant();

            switch (extension)
            {
                case ".png":
                    return "image/png";
                case ".jpg":
                    return "image/jpg";
                case ".jpeg":
                    return "image/jpeg";
                default:
                    return "application/octet-stream";
            }
        }

        public async void uploadFile(string imageFilePath, IFormFile imageFile)
        {
            using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
        }

    }
}
