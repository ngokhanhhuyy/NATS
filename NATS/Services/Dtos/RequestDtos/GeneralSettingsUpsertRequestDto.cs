namespace NATS.Services.Dtos.RequestDtos;

public class GeneralSettingsUpsertRequestDto : IRequestDto<GeneralSettingsUpsertRequestDto>
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public byte[] FavIconFile { get; set; }
    public bool UnderMaintainance { get; set; }

    public GeneralSettingsUpsertRequestDto TransformValues()
    {
        ApplicationName = ApplicationName.ToNullIfEmpty();
        ApplicationShortName = ApplicationShortName.ToNullIfEmpty();
        return this;
    }
}
