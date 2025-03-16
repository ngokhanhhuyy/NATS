namespace NATS.Services.Dtos.ResponseDtos;

public class GeneralSettingsResponseDto
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public string FavIconUrl { get; set; }
    public bool UnderMaintainance { get; set; }

    public GeneralSettingsResponseDto(GeneralSettings settings)
    {
        ApplicationName = settings.ApplicationName;
        ApplicationShortName = settings.ApplicationShortName;
        FavIconUrl = settings.FavIconUrl;
        UnderMaintainance = settings.UnderMaintainance;
    }
}
