using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WebAPI.IRepository;

namespace WebAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IConfiguration config)
        {
            Account account = new Account(
                config.GetSection("CloudinarySettings:CloudName").Value,
                config.GetSection("CloudinarySettings:APIKey").Value,
                config.GetSection("CloudinarySettings:APISecret").Value
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {

            var uploadResult = new ImageUploadResult();
            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500).Width(800)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;

            //var uploadResult = new ImageUploadResult();
            //if(photo.Length > 0){

            //    Stream stream;
            //    ImageUploadParams uploadParams;
            //    using (stream = photo.OpenReadStream())
            //    {
            //        uploadParams = new ImageUploadParams{
            //            File = new FileDescription(photo.FileName, stream),
            //            Transformation = new Transformation()
            //                .Height(500).Width(800)
            //        };
            //    }

            //    uploadResult = await _cloudinary.UploadAsync(uploadParams);                
            //}
            //return uploadResult;
        }
    }
}