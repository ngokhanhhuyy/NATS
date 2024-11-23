namespace NATS.Services.Dtos.ResponseDtos;

public class GeneralSettingsResponseDto
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public string FavIconUrl { get; set; }
    public bool UnderMaintainance { get; set; }
}
