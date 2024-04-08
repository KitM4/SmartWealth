using CloudinaryDotNet.Actions;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface ICloudinaryService
{
    public Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);

    public Task<DeletionResult> DeletePhotoAsync(string publicId);
}