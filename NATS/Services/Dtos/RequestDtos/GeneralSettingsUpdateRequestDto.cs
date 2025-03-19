namespace NATS.Services.Dtos.RequestDtos;

public class GeneralSettingsUpdateRequestDto : IRequestDto
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public byte[] FavIconFile { get; set; }
    public bool UnderMaintainance { get; set; }

    public void TransformValues()
    {
        ApplicationName = ApplicationName.ToNullIfEmpty();
        ApplicationShortName = ApplicationShortName.ToNullIfEmpty();
    }
}
