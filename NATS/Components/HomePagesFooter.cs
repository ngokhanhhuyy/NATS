namespace NATS.Components;

public class HomePagesFooter : ViewComponent
{
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IPostService _postService;
    private readonly IContactInfoService _contactInfoService;

    public HomePagesFooter(
            IGeneralSettingsService generalSettingsService,
            IPostService postService,
            IContactInfoService contactInfoService)
    {
        _generalSettingsService = generalSettingsService;
        _postService = postService;
        _contactInfoService = contactInfoService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Get general settings data.
        ServiceResult<GeneralSettingsResponseDto> generalSettingsServiceResult;
        generalSettingsServiceResult = await _generalSettingsService.GetAsync();

        // Get top 3 lastest post.
        ServiceResult<List<PostBasicResponseDto>> postServiceResult;
        postServiceResult = await _postService.GetLastestBasicListAsync(3);

        // Get the contact info.
        ServiceResult<ContactInfoResponseDto> contactInfoServiceResult;
        contactInfoServiceResult = await _contactInfoService.GetAsync();



        FooterViewModel model = new FooterViewModel
        {
            GeneralSettings = new GeneralSettingsViewModel
            {
                ApplicationName = generalSettingsServiceResult.ResponseDto.ApplicationName,
                ApplicationShortName = generalSettingsServiceResult.ResponseDto.ApplicationShortName
            },
            Posts = new PostBasicListViewModel
            {
                Items = postServiceResult.ResponseDto
                    .Select(p => new PostBasicViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        NormalizedTitle = p.NormalizedTitle,
                        ThumbnailUrl = p.ThumbnailUrl,
                        Content = p.Content,
                        CreatedDateTime = p.CreatedDateTime,
                        IsPublished = p.IsPublished,
                        IsPinned = p.IsPinned,
                        Views = p.Views
                    }).ToList()
            },
            ContactInfo = new ContactInfoViewModel
            {
                PhoneNumber = contactInfoServiceResult.ResponseDto.PhoneNumber,
                ZaloNumber = contactInfoServiceResult.ResponseDto.ZaloNumber,
                Email = contactInfoServiceResult.ResponseDto.Email,
                Address = contactInfoServiceResult.ResponseDto.Address,
            }
        };
        
        return View(model);
    }
}