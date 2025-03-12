namespace NATS.Models;

public class GeneralSettingsDetailModel
{
    [Display(Name = DisplayNames.ApplicationName)]
    public string ApplicationName { get; set; }

    [Display(Name = DisplayNames.ApplicationShortName)]
    public string ApplicationShortName { get; set; }

    [Display(Name = DisplayNames.FavIcon)]
    public string FavIconUrl { get; set; }

    [Display(Name = DisplayNames.UnderMaintainance)]
    public bool UnderMaintainance { get; set; }

    public GeneralSettingsDetailModel(GeneralSettingsResponseDto responseDto)
    {
        ApplicationName = responseDto.ApplicationName;
        ApplicationShortName = responseDto.ApplicationShortName;
        FavIconUrl = responseDto.FavIconUrl;
        UnderMaintainance = responseDto.UnderMaintainance;
    }
}
