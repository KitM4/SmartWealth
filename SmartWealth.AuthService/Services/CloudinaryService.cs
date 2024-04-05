using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SmartWealth.AuthService.Utilities.Cloudinary;
using Microsoft.Extensions.Options;

namespace SmartWealth.AuthService.Services;

public class CloudinaryService(IOptions<CloudinarySettings> options) : ICloudinaryService
{
    private readonly Cloudinary _cloudinary = new(new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret));

    public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
    {
        ImageUploadResult result = new();

        if (file.Length > 0)
        {
            using Stream stream = file.OpenReadStream();
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(150).Width(150).Crop("fill").Gravity("face"),
            };

            result = await _cloudinary.UploadAsync(uploadParams);
        }

        return result;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        return await _cloudinary.DestroyAsync(new(publicId));
    }
}