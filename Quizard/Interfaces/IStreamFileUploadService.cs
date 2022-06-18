using Microsoft.AspNetCore.WebUtilities;

namespace Quizard.Interfaces
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section);
    }
}
