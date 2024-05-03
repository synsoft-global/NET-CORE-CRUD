using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CoreApiWithEntity.BLL.Interface;

namespace CoreApiWithEntity.BLL.Repository
{  
    public class ImageRepo : IImage
    {
        private readonly string _uploadDirectory;

        public ImageRepo(IConfiguration configuration)
        {
            _uploadDirectory = configuration["ImageUploadDirectory"];

            if (string.IsNullOrEmpty(_uploadDirectory))
            {
                _uploadDirectory = "E:\\Yagya\\CoreApiWithEntity\\CoreApiWithEntity\\wwwroot";
            }

            if (!Directory.Exists(_uploadDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_uploadDirectory);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create upload directory: {_uploadDirectory}. Error: {ex.Message}");
                }
            }
        }
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is required.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream); // Await the CopyToAsync method
            }

            return "/uploads/" + fileName; // Return the relative URL
        }
    }
}
