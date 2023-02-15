using CloudinaryDotNet.Actions;

namespace WebAPI.IRepository
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo);
        
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}