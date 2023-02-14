using CloudinaryDotNet.Actions;

namespace WebAPI.IRepository
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo);
        //Will add one more method for deleting the photo.
    }
}