namespace NATS.Controllers;

[Route("quan-tri/bai-viet")]
[Authorize]
public class AdminPostController : Controller
{
    private readonly IPostService _service;
    
    public AdminPostController(IPostService service)
    {
        _service = service;
    }

    [HttpGet("trang-{page:int}")]
    public async Task<IActionResult> List(int page)
    {
        ServiceResult<PostListStatisticsResponseDto> statisticsServiceResult;
        statisticsServiceResult = await _service.GetStatisticsAsync();

        ServiceResult<PostBasicListResponseDto> listServiceResult;
        listServiceResult = await _service.GetBasicListAsync(page);

        // Initialize view model and map data from response dto
        PostBasicListViewModel model = new PostBasicListViewModel
        {
            Statistics = new PostListStatisticsViewModel
            {
                TotalPosts = statisticsServiceResult.ResponseDto.TotalPosts,
                TotalViews = statisticsServiceResult.ResponseDto.TotalViews,
                UnpublishedPosts = statisticsServiceResult.ResponseDto.UnpublishedPosts,
            },
            Items = listServiceResult.ResponseDto.Items
                .Select(p => new PostBasicViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ThumbnailUrl = p.ThumbnailUrl,
                    Content = p.Content,
                    CreatedDateTime = p.CreatedDateTime,
                    IsPublished = p.IsPublished,
                    IsPinned = p.IsPinned,
                    Views = p.Views
                }).ToList(),
            Page = page,
            PageCount = listServiceResult.ResponseDto.PageCount
        };
        return View("~/Views/Admin/Post/PostList.cshtml", model);
    }
    
    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        // Initialize view model
        PostDetailViewModel model = new PostDetailViewModel
        {
            IsForCreating = true
        };
        return View("~/Views/Admin/Post/Post.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(PostDetailViewModel model)
    {
        // Convert thumbnail file from IFormFile to byte[].
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }

        // Map data from view model to request dto.
        PostDetailRequestDto requestDto = new PostDetailRequestDto
        {
            Title = model.Title,
            Content = model.Content,
            IsPinned = model.IsPinned,
            IsPublished = model.IsPublished,
            ThumbnailChanged = model.ThumbnailChanged,
            ThumbnailFile = thumbnailFile
        };

        // Perform the creating operation.
        ServiceResult<PostDetailResponseDto> serviceResult;
        serviceResult = await _service.CreateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }
        return Ok();
    }
    
    [HttpGet("{id:int}/cap-nhat")]
    public async Task<IActionResult> Updating(int id)
    {
        ServiceResult<PostDetailResponseDto> serviceResult;
        serviceResult = await _service.GetDetailAsync(id, viewsIncrement: true);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        PostDetailViewModel model = new PostDetailViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Title = serviceResult.ResponseDto.Title,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
            Content = serviceResult.ResponseDto.Content,
            CreatedDateTime = serviceResult.ResponseDto.CreatedDateTime,
            UpdatedDateTime = serviceResult.ResponseDto.UpdatedDateTime,
            IsPinned = serviceResult.ResponseDto.IsPinned,
            IsPublished = serviceResult.ResponseDto.IsPublished,
            Views = serviceResult.ResponseDto.Views,
            User = new UserBasicViewModel
            {
                Id = serviceResult.ResponseDto.User.Id,
                UserName = serviceResult.ResponseDto.User.UserName
            }
        };
        return View("~/Views/Admin/Post/Post.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, PostDetailViewModel model)
    {
        // Convert thumbnail file from IFormFile to byte[].
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }

        // Map data from view model to request dto.
        PostDetailRequestDto requestDto = new PostDetailRequestDto
        {
            Title = model.Title,
            Content = model.Content,
            IsPinned = model.IsPinned,
            IsPublished = model.IsPublished,
            ThumbnailChanged = model.ThumbnailChanged,
            ThumbnailFile = thumbnailFile
        };

        // Perform the update operation.
        ServiceResult<PostDetailResponseDto> serviceResult;
        serviceResult = await _service.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPost("{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deleting(int id)
    {
        ServiceResult<int> serviceResult = await _service.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        return Ok();
    }
}