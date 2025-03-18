namespace NATS.FrontPages.Models;

public class ContactViewModel
{
    public List<ContactDetailModel> Contacts { get; set; }
    public GeneralSettingsDetailModel GeneralSettings { get; set; }

    public ContactViewModel(
            List<ContactResponseDto> contactResponseDtos,
            GeneralSettingsResponseDto generalSettingsResponseDtos)
    {
        Contacts = contactResponseDtos
            .Select(dto => new ContactDetailModel(dto))
            .ToList();
        GeneralSettings = new GeneralSettingsDetailModel(generalSettingsResponseDtos);
    }
}