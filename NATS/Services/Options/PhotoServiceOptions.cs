namespace NATS.Services.Options;

public class PhotoServiceOptions
{
    public int? DesiredWidth { get; set; } = null;
    public int? DesiredHeight { get; set; } = null;
    public double? DesiredAspectRatio { get; set; } = null;
}