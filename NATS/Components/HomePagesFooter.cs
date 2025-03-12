namespace NATS.Components;

public class HomePagesFooter : ViewComponent
{
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IPostService _postService;
    private readonly IContactService _contactService;

    public HomePagesFooter(
            IGeneralSettingsService generalSettingsService,
            IPostService postService,
            IContactService contactService)
    {
        _generalSettingsService = generalSettingsService;
        _postService = postService;
        _contactService = contactService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Get general settings data.
        Task<GeneralSettingsResponseDto> generalSettingsTask;
        generalSettingsTask = _generalSettingsService.GetAsync();

        // Get top 3 lastest post.
        Task<PostListResponseDto> postListTask = _postService.GetListAsync(1, 3);

        // Get the contact info.
        Task<List<ContactResponseDto>> contactsTask = _contactService.GetListAsync();

        await Task.WhenAll(generalSettingsTask, postListTask, contactsTask);

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
            ContactInfo = new ContactModel
            {
                PhoneNumber = contactsTask.ResponseDto.PhoneNumber,
                ZaloNumber = contactsTask.ResponseDto.ZaloNumber,
                Email = contactsTask.ResponseDto.Email,
                Address = contactsTask.ResponseDto.Address,
            }
        };
        
        return View(model);
    }
}