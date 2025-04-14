namespace NATS.Models;

public class NavigationBarViewModel
{
    public GeneralSettingsDetailModel GeneralSettings { get; set; }

    public NavigationBarViewModel(GeneralSettingsResponseDto generalSettingsResponseDto)
    {
        GeneralSettings = new GeneralSettingsDetailModel(generalSettingsResponseDto);
    }
}