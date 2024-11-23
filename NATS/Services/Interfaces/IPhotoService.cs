namespace NATS.Services.Interfaces;

public interface IPhotoService
{
    Task<ServiceResult<string>> CreateAsync(
        byte[] content,
        string folderName,
        bool cropToSquare = false);

    Task<ServiceResult<string>> CreateAsync(
        byte[] content,
        string folderName,
        double aspectRatio);
    
    ServiceResult<string> Delete(string relativePath);
}