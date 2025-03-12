namespace NATS.Models;

public class FooterModel
{
    [Display(Name = DisplayNames.GeneralSettings)]
    public GeneralSettingsUpsertModel GeneralSettings { get; set; }

    [Display(Name = DisplayNames.Post)]
    public PostListModel PostList { get; set; }

    [Display(Name = DisplayNames.Contact)]
    public List<ContactDetailModel> Contacts { get; set; }

    public FooterModel(
            GeneralSettingsResponseDto generalSettingsResponseDto,
            PostListResponseDto postListResponseDto,
            List<ContactResponseDto> contactResponseDtos)
    {
        GeneralSettings = new GeneralSettingsUpsertModel(generalSettingsResponseDto);
        PostList = new PostListModel(postListResponseDto);
        Contacts = contactResponseDtos.Select(dto => new ContactDetailModel(dto)).ToList();
    }
}