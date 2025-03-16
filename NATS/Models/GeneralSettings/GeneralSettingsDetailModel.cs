namespace NATS.Models;

public class GeneralSettingsDetailModel
{
    public string ApplicationName { get; set; }
    public string ApplicationShortName { get; set; }
    public bool UnderMaintainance { get; set; }

    public GeneralSettingsDetailModel(GeneralSettingsResponseDto responseDto)
    {
        ApplicationName = responseDto.ApplicationName;
        ApplicationShortName = responseDto.ApplicationShortName;
        UnderMaintainance = responseDto.UnderMaintainance;
    }
}