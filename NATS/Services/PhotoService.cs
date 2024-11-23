using System.ComponentModel;

namespace NATS.Services;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _environment;

    public PhotoService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>
    /// Create a photo and save on the photo folder with subfolder {ntityName}.
    /// The name of the photo will be the time when this method is called.
    /// </summary>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="folderName">
    /// The name of the folder inside /images/front-pages directory that this photo will be saved.
    /// </param>
    /// <param name="cropToSquare">
    /// Determine if the image should be cropped into square image.
    /// </param>
    /// <returns>The relative path (URL) to the created photo on the server</returns>
    /// <example>~/photos/users/{id}.jpg</example>
    public async Task<ServiceResult<string>> CreateAsync(
            byte[] content,
            string folderName,
            bool cropToSquare)
    {
        MagickImage image = new MagickImage(content);

        // Process image's size
        ResizeImageIfTooLarge(image);
        if (cropToSquare)
        {
            CropIntoSquareImage(image);
        }

        // Determine the path where the image would be saved
        string path = Path.Combine(
            _environment.WebRootPath,
            "images",
            "front-pages",
            folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        string relativeFilePath = "/" + Path.Combine(
            "images",
            "front-pages",
            folderName,
            fileName);
        return ServiceResult<string>.Success(relativeFilePath);
    }

    /// <summary>
    /// Create a photo and save on the photo folder with subfolder {ntityName}.
    /// The name of the photo will be the time when this method is called.
    /// </summary>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="folderName">
    /// The name of the folder inside /images/front-pages directory that this photo will be saved.
    /// </param>
    /// <param name="options">
    /// The object contains the desired with, height and aspect ratio of the image after being processed.
    /// </param>
    /// <returns>The relative path (URL) to the created photo on the server</returns>
    /// <example>~/photos/users/{id}.jpg</example>
    public async Task<ServiceResult<string>> CreateAsync(
            byte[] content,
            string folderName,
            double aspectRatio)
    {
        MagickImage image = new MagickImage(content);

        CropToAspectRatio(image, aspectRatio);

        // Determine the path where the image would be savedv
        string path = Path.Combine(
            _environment.WebRootPath,
            "images",
            "front-pages",
            folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        string relativeFilePath = "/" + Path.Combine(
            "images",
            "front-pages",
            folderName,
            fileName);
        return ServiceResult<string>.Success(relativeFilePath);
    }

    /// <summary>
    /// Delete an existing photo by relative path on the server. The relative path is usually stored in
    /// the database and associated with some of the main entities.
    /// </summary>
    /// <param name="relativePath">
    /// The relative path to the photo on the server, usually in wwwroot/photos/{entityName}/
    /// </param>
    /// <returns>The old relative path (URL) of the photo before being deleted.</return>
    public ServiceResult<string> Delete(string relativePath)
    {
        List<string> pathElements = new List<string> { _environment.WebRootPath };
        string[] testingArray = relativePath.Split("/");
        pathElements.AddRange(relativePath.Split("/").Skip(1));
        string path = Path.Combine(pathElements.ToArray());

        if (!File.Exists(path))
        {
            return ServiceResult<string>.Failed(ServiceError.NotFound("PhotoFile"));
        }

        File.Delete(path);
        return ServiceResult<string>.Success(path);
    }

    /// <summary>
    /// Resize an image if either of width or height, or both of them, exceeds the maximum pixel value (1024px)
    /// while keeping the aspect ratio.
    /// The resized image will also be converted into JPEG format.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and resized.
    /// </param>
    private static void ResizeImageIfTooLarge(IMagickImage image, int maxWidth = 1024, int maxHeight = 1024)
    {
        image.Quality = 100;
        image.Format = MagickFormat.Jpeg;
        double widthHeightRatio = (double)image.Width / image.Height;
        // Checking if image width or height or both exceeds maximum size
        if (image.Width > maxWidth || image.Height > maxHeight) {
            int newWidth, newHeight;
            // Width is greater than height, cropping the left and the right sides of the image
            if (widthHeightRatio > 1) {
                newHeight = maxHeight;
                newWidth = (int)Math.Round(newHeight * widthHeightRatio);
            } else {
                newWidth = maxWidth;
                newHeight = (int)Math.Round(newWidth / widthHeightRatio);
            }
            image.Resize(newWidth, newHeight);
        }
    }

    /// <summary>
    /// Resize an image to the desired aspect ratio.
    /// The width or height, which one has greater value, will remain.
    /// The other's value will be calculated based on the desired aspect ratio.
    /// The resized image will also be converted into JPEG format.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and resized.
    /// </param>
    /// <param name="desiredAspectRatio">
    /// The desired aspect ratio of the image after being cropped.
    /// </param>
    private static void CropToAspectRatio(IMagickImage image, double desiredAspectRatio)
    {
        double originalAspectRatio = (double)image.Width / image.Height;
        // Determine which one of width and height is larger.
        MagickGeometry geometry;
        if (desiredAspectRatio >= originalAspectRatio)
        {
            double croppedHeight = image.Width / desiredAspectRatio;
            geometry = new MagickGeometry(
                0,
                (int)Math.Round((image.Height - croppedHeight) / 2),
                image.Width,
                (int)Math.Round(croppedHeight));
            image.Crop(geometry);
        }
        else
        {
            double croppedWidth = image.Height * desiredAspectRatio;
            geometry = new MagickGeometry(
                (int)Math.Round((image.Width - croppedWidth) / 2),
                0,
                (int)Math.Round(croppedWidth),
                image.Height);
            image.Crop(geometry);
        }
    }

    /// <summary>
    /// Crop an image into square. The geomery of the part which is kept after cropping is the center of the image.
    /// The size after being cropped will equal to original image's width or height, based on which one is smaller.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and cropped.
    /// </param>
    private static void CropIntoSquareImage(IMagickImage image)
    {
        // Crop image if needed to make sure it's square
        if (image.Width != image.Height) {
            int size = Math.Min(image.Width, image.Height);
            int x, y;
            if (image.Width > image.Height) {
                x = (int)Math.Round((double)(image.Width - image.Height) / 2);
                y = 0;
            } else {
                x = 0;
                y = (int)Math.Round((double)(image.Height - image.Width) / 2);
            }
            image.Crop(new MagickGeometry(x, y, size, size));
        }
    }
}
