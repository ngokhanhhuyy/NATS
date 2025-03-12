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

        FooterModel model = new FooterModel(
            generalSettingsTask.Result,
            postListTask.Result,
            contactsTask.Result);
        
        return View(model);
    }
}