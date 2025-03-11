namespace NATS.Services;

/// <inheritdoc />
public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _environment;

    public PhotoService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <inheritdoc />
    public async Task<string> CreateAsync(byte[] content, bool cropToSquare)
    {
        MagickImage image = new(content);

        // Process image's size.
        ResizeImageIfTooLarge(image);
        if (cropToSquare)
        {
            CropIntoSquareImage(image);
        }

        // Determine the path where the image would be saved.
        string path = Path.Combine(_environment.WebRootPath, "images", "data");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        return "/" + Path.Combine("images", "data", fileName);
    }

    /// <inheritdoc />
    public async Task<string> CreateAsync(byte[] content, double aspectRatio)
    {
        MagickImage image = new(content);

        CropToAspectRatio(image, aspectRatio);

        // Determine the path where the image would be savedv
        string path = Path.Combine(_environment.WebRootPath, "images", "data");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        return "/" + Path.Combine("images", "data", fileName);
    }

    /// <inheritdoc />
    public void Delete(string relativePath)
    {
        List<string> pathElements = new() { _environment.WebRootPath };
        pathElements.AddRange(relativePath.Split("/").Skip(1));
        string path = Path.Combine(pathElements.ToArray());

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Resizes an image if either of width or height, or both of them, exceeds the maximum
    /// pixel value (1024px) while keeping the aspect ratio.
    /// </summary>
    /// <remarks>
    /// The resized image will also be converted into JPEG format.
    /// </remarks>
    /// <param name="image">
    /// An instance of the <see cref="MagickImage"/> class loaded from a byte array which is to
    /// be checked and resized.
    /// </param>
    /// <param name="maxWidth">
    /// The maximum width that the image will be resized to if exceeding.
    /// </param>
    /// <param name="maxHeight">
    /// The maximum height that the image will be resized to if exceeding.
    /// </param>
    private static void ResizeImageIfTooLarge(
            MagickImage image,
            uint maxWidth = 1024,
            uint maxHeight = 1024)
    {
        image.Quality = 100;
        image.Format = MagickFormat.Jpeg;
        double widthHeightRatio = (double)image.Width / image.Height;

        // Checking if image width or height or both exceeds maximum size
        if (image.Width > maxWidth || image.Height > maxHeight) {
            uint newWidth, newHeight;
            // Cropping the left and the right sides of the image when its width is greater
            // its than height.
            if (widthHeightRatio > 1) {
                newHeight = maxHeight;
                newWidth = (uint)Math.Round(newHeight * widthHeightRatio);
            } else {
                newWidth = maxWidth;
                newHeight = (uint)Math.Round(newWidth / widthHeightRatio);
            }

            image.Resize(newWidth, newHeight);
        }
    }

    /// <summary>
    /// Resizes an image to the desired aspect ratio.
    /// </summary>
    /// <remarks>
    /// The width or height, which one has greater value, will remain. The other's value will
    /// be calculated based on the desired aspect ratio. The resized image will also be
    /// converted into JPEG format.
    /// </remarks>
    /// <param name="image">
    /// An instance of the <see cref="MagickImage"/> class loaded from a byte array which is to
    /// be checked and resized.
    /// </param>
    /// <param name="desiredAspectRatio">
    /// The desired aspect ratio of the image after being cropped.
    /// </param>
    private static void CropToAspectRatio(MagickImage image, double desiredAspectRatio)
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
                (uint)Math.Round(croppedHeight));
            image.Crop(geometry);
        }
        else
        {
            double croppedWidth = image.Height * desiredAspectRatio;
            geometry = new MagickGeometry(
                (int)Math.Round((image.Width - croppedWidth) / 2),
                0,
                (uint)Math.Round(croppedWidth),
                image.Height);
            image.Crop(geometry);
        }
    }

    /// <summary>
    /// Crops an image into a square one.
    /// </summary>
    /// <remarks>
    /// The geomery of the part which is kept after cropping is the center of the image. The
    /// size after being cropped will equal to original image's width or height, based on which
    /// one is smaller.
    /// </remarks>
    /// <param name="image">
    /// An instance of the <see cref="IMagickImage"/> class, loaded from byte array which is to
    /// be checked and cropped.
    /// </param>
    private static void CropIntoSquareImage(MagickImage image)
    {
        // Crop image if needed to make sure it's square
        if (image.Width != image.Height) {
            uint size = image.Width < image.Height ? image.Width : image.Height;
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
