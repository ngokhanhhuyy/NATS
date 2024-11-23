namespace NATS.Services.Dtos.RequestDtos;

public class GeneralSettingsRequestDto : IRequestDto<GeneralSettingsRequestDto>
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public byte[] FavIconFile { get; set; }
    public bool UnderMaintainance { get; set; }

    public GeneralSettingsRequestDto TransformValues()
    {
        ApplicationName = ApplicationName.ToNullIfEmpty();
        ApplicationShortName = ApplicationShortName.ToNullIfEmpty();
        return this;
    }
}
