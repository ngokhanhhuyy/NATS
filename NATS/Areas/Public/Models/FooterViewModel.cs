using System.Security.Claims;

namespace NATS.Public.Models;

public class FooterViewModel
{
    public GeneralSettingsDetailModel GeneralSettings { get; set; }
    public List<ContactDetailModel> Contacts { get; set; }
    
    public FooterViewModel(
            GeneralSettingsResponseDto generalSettingsResponseDto,
            List<ContactResponseDto> contactResponseDtos)
    {
        GeneralSettings = new GeneralSettingsDetailModel(generalSettingsResponseDto);
        Contacts = contactResponseDtos
            .Select(dto => new ContactDetailModel(dto))
            .ToList();
    }

    public bool IsAuthenticated(ClaimsPrincipal user)
    {
        return user.Identity.IsAuthenticated;
    }
}