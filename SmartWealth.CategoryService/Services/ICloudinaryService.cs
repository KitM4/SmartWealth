using CloudinaryDotNet.Actions;

namespace SmartWealth.CategoryService.Services;

public interface ICloudinaryService
{
    public Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);

    public Task<DeletionResult> DeletePhotoAsync(string publicId);
}