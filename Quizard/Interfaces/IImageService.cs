using CloudinaryDotNet.Actions;

namespace Quizard.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> AddImage(IFormFile file);
        Task<DeletionResult> DeleteImage(string publicUrl);
    }
}
