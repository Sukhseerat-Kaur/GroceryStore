using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IImageService
    {
        public string GetFilePath(string contentRootPath, IFormFile ImageFile);
        void uploadFile(string imageFilePath, IFormFile imageFile);
        string GetImageContentType(string imageFilePath);
        byte[] GetFileBytes(string imageFilePath);
    }
}
