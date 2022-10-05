using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Quizard.Interfaces;

namespace Quizard.Services
{
    public class ImageService :IImageService
    {
        private readonly Cloudinary _cloundinary;
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
            var account = new Account(
                _configuration["CloudinaryAcc:CloudName"],
                _configuration["CloudinaryAcc:ApiKey"],
                _configuration["CloudinaryAcc:ApiSecret"]
                );

            _cloundinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams();

                uploadParams.File = new FileDescription(file.FileName, stream);
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImage(string contentUrl)
        {
            var publicId = contentUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return await _cloundinary.DestroyAsync(deleteParams);
        }
    }
}
