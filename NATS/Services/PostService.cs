namespace NATS.Services;

public class PostService : IPostService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService _photoService;
    private readonly IUserService _userService;
    private readonly IValidator<PostDetailRequestDto> _validator;
    
    public PostService(
            DatabaseContext context,
            IPhotoService photoService,
            IUserService userService,
            IValidator<PostDetailRequestDto> validator)
    {
        _context = context;
        _photoService = photoService;
        _userService = userService;
        _validator = validator;
    }

    /// <summary>
    /// Get a list of all posts' with basic information.
    /// </summary>
    /// <returns>A list of objects containing all posts' basic information.</returns>
    public async Task<ServiceResult<PostBasicListResponseDto>> GetBasicListAsync(int page)
    {
        const int resultPerPage = 15;
        // Determine the page count.
        int pageCount;
        int postCount = await _context.Posts.CountAsync();
        if (postCount == 0)
        {
            pageCount = 0;
        }
        else
        {
            pageCount = (int)Math.Ceiling((double)postCount / resultPerPage);
        }
        // Preparing query statement
        
        // Initialize response dto and map data from entities to the dto.
        PostBasicListResponseDto responseDto = new PostBasicListResponseDto
        {
            Items = await _context.Posts
                .OrderBy(p => p.IsPinned)
                .ThenBy(p => p.Id)
                .ThenBy(p => p.NormalizedTitle)
                .Select(p => new PostBasicResponseDto
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
                }).Skip((page - 1) * resultPerPage)
                .Take(resultPerPage)
                .ToListAsync(),
            PageCount = pageCount
        };
        
        // Return the response dto.
        return ServiceResult<PostBasicListResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Get a list of lastest posts by specified number of posts with basic information.
    /// </summary>
    /// <param name="limit">The maximum number of the results.</param>
    /// <returns>A list of objects containing the basic data of the lastest posts.</returns>
    public async Task<ServiceResult<List<PostBasicResponseDto>>> GetLastestBasicListAsync(int limit)
    {
        List<PostBasicResponseDto> responseDtos = await _context.Posts
            .OrderByDescending(p => p.CreatedDateTime)
            .Select(p => new PostBasicResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                NormalizedTitle = p.NormalizedTitle,
                ThumbnailUrl = p.ThumbnailUrl,
                Content = p.Content,
                CreatedDateTime = p.CreatedDateTime,
                IsPinned = p.IsPinned,
                IsPublished = p.IsPublished,
                Views = p.Views
            }).Take(limit)
            .ToListAsync();
        
        return ServiceResult<List<PostBasicResponseDto>>.Success(responseDtos);
    }
    
    /// <summary>
    /// Get a list of all posts' detail information.
    /// </summary>
    /// <returns>A list containing of posts' information</returns>
    public async Task<ServiceResult<List<PostDetailResponseDto>>> GetDetailListAsync()
    {
        List<PostDetailResponseDto> responseDtos = await _context.Posts
            .OrderBy(p => p.IsPinned)
            .ThenBy(p => p.Id)
            .ThenBy(p => p.NormalizedTitle)
            .Select(p => new PostDetailResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                NormalizedTitle = p.NormalizedTitle,
                ThumbnailUrl = p.ThumbnailUrl,
                Content = p.Content,
                CreatedDateTime = p.CreatedDateTime,
                UpdatedDateTime = p.UpdatedDateTime,
                IsPublished = p.IsPublished,
                IsPinned = p.IsPinned,
                Views = p.Views,
                User = new UserBasicResponseDto
                {
                    Id = p.User.Id,
                    UserName = p.User.UserName
                }
            }).ToListAsync();
        return ServiceResult<List<PostDetailResponseDto>>.Success(responseDtos);
    }
    
    /// <summary>
    /// Get the detail information of a specific post with given id.
    /// </summary>
    /// <param name="id">The id of the post.</param>
    /// <param name="viewsIncrement">
    /// Determine if the value of the view property of the post should be incremented
    /// </param>
    /// <returns>An object containing all the detail information of the post.</returns>
    public async Task<ServiceResult<PostDetailResponseDto>> GetDetailAsync(int id, bool viewsIncrement = false)
    {
        // Fetch the entity with given id from the database
        Post post = await _context.Posts
            .Include(p => p.User)
            .ThenInclude(u => u.Roles)
            .SingleOrDefaultAsync(p => p.Id == id);
            
        // Ensure the entity exists in the database
        if (post == null)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Post),
                    nameof(id),
                    id.ToString()));
        }
        
        // Increment views if specified
        if (viewsIncrement)
        {
            post.Views += 1;
            await _context.SaveChangesAsync();
        }
        
        // Map entity's data to the response dto
        PostDetailResponseDto responseDto = new PostDetailResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            NormalizedTitle = post.NormalizedTitle,
            ThumbnailUrl = post.ThumbnailUrl,
            Content = post.Content,
            CreatedDateTime = post.CreatedDateTime,
            UpdatedDateTime = post.UpdatedDateTime,
            IsPinned = post.IsPinned,
            IsPublished = post.IsPublished,
            Views = post.Views,
            User = new UserBasicResponseDto
            {
                Id = post.User.Id,
                UserName = post.User.UserName,
                Role = new RoleResponseDto
                {
                    Id = post.User.Role.Id,
                    Name = post.User.Role.Name
                }
            }
        };
        
        // Return the response dto
        return ServiceResult<PostDetailResponseDto>.Success(responseDto);
    }
    
    /// <summary>
    /// Get the detail information of a specific post with given normalized title.
    /// </summary>
    /// <param name="normalizedTitle">The title of the post representing the url of the post.</param>
    /// <param name="viewsIncrement">
    /// Determine if the value of the view property of the post should be incremented
    /// </param>
    /// <returns>An object containing all the detail information of the post.</returns>
    public async Task<ServiceResult<PostDetailResponseDto>> GetDetailAsync(
            string normalizedTitle,
            bool viewsIncrement = true)
    {
        // Ensure specified normalized title is not null.
        if (normalizedTitle == null)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(
                ServiceError.NotFound(nameof(Post)));
        }

        // Fetch the entity from the database.
        Post post = await _context.Posts
            .SingleOrDefaultAsync(p => p.NormalizedTitle == normalizedTitle);
        
        // Ensure the entity exists in the database.
        if (post == null)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Post),
                    nameof(normalizedTitle),
                    normalizedTitle.ToString()));
        }

        // Increment post's views value if specified.
        if (viewsIncrement)
        {
            post.Views += 1;
            await _context.SaveChangesAsync();
        }

        // Initialize response dto.
        PostDetailResponseDto responseDto = new PostDetailResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            NormalizedTitle = post.NormalizedTitle,
            ThumbnailUrl = post.ThumbnailUrl,
            Content = post.Content,
            CreatedDateTime = post.CreatedDateTime,
            UpdatedDateTime = post.UpdatedDateTime,
            IsPinned = post.IsPinned,
            IsPublished = post.IsPublished,
            Views = post.Views,
            User = new UserBasicResponseDto
            {
                Id = post.User.Id,
                UserName = post.User.UserName,
                Role = new RoleResponseDto
                {
                    Id = post.User.Role.Id,
                    Name = post.User.Role.Name,
                    DisplayName = post.User.Role.DisplayName
                }
            }
        };

        return ServiceResult<PostDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Get statistics figures of all categories which contains total categories count,
    /// total posts count, total views count.
    /// </summary>
    /// <returns>An object containing data of the statistics.</returns>
    public async Task<ServiceResult<PostListStatisticsResponseDto>> GetStatisticsAsync()
    {
        PostListStatisticsResponseDto responseDto = await _context.Posts
            .Select(_ => new PostListStatisticsResponseDto
            {
                TotalPosts = _context.Posts.Count(),
                TotalViews = _context.Posts.Sum(p => p.Views),
                UnpublishedPosts = _context.Posts.Count(p => !p.IsPublished),
            }).Take(1)
            .SingleAsync();
        return ServiceResult<PostListStatisticsResponseDto>.Success(responseDto);
    }
    
    /// <summary>
    /// Create a post with the data provided from the request.
    /// </summary>
    /// <param name="requestDto">An object containing all the data for a new post.</param>
    /// <returns>An object containing all the detail information of the created post.</returns>
    public async Task<ServiceResult<PostDetailResponseDto>> CreateAsync(
            PostDetailRequestDto requestDto)
    {
        // Validate data from request.
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(result.Errors);
        }

        // Create a new thumbnail file if the request contains data for one.
        string thumbnailUrl = null;
        if (requestDto.ThumbnailFile != null)
        {
            ServiceResult<string> photoServiceResult = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "posts",
                true);
            thumbnailUrl = photoServiceResult.ResponseDto;
        }

        // Initialize the entity.
        Post post = new Post
        {
            Title = requestDto.Title,
            NormalizedTitle = await GenerateNormalizedTitle(requestDto.Title),
            ThumbnailUrl = thumbnailUrl,
            Content = requestDto.Content,
            IsPinned = requestDto.IsPinned,
            IsPublished = requestDto.IsPublished,
            UserId = _userService.GetUserAsCurrentUser().ResponseDto.Id,
        };
        _context.Posts.Add(post);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the created post.
        PostDetailResponseDto responseDto = new PostDetailResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            NormalizedTitle = post.NormalizedTitle,
            ThumbnailUrl = post.ThumbnailUrl,
            Content = post.Content,
            IsPinned = post.IsPinned,
            IsPublished = post.IsPublished,
            User = new UserBasicResponseDto
            {
                Id = post.User.Id,
                UserName = post.User.UserName
            }
        };
        return ServiceResult<PostDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update a post with the id and data provided from the request.
    /// </summary>
    /// <param name="id">The id of the post to be updated.</param>
    /// <param name="requestDto">An object containing all the data to be updated.</param>
    /// <returns>An object containing all the detail information of the updated post.</returns>
    public async Task<ServiceResult<PostDetailResponseDto>> UpdateAsync(
            int id,
            PostDetailRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity with given id from the database.
        Post post = await _context.Posts
            .Include(p => p.User)
            .SingleOrDefaultAsync(p => p.Id == id);

        // Ensure the entity exists in the database.
        if (post == null)
        {
            return ServiceResult<PostDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Post),
                    nameof(id),
                    id.ToString()));
        }

        // Update the thumbnail if the request indicated.
        if (requestDto.ThumbnailChanged)
        {
            // Remove the old thumbnail if there is any.
            if (post.ThumbnailUrl != null)
            {
                _photoService.Delete(post.ThumbnailUrl);
                post.ThumbnailUrl = null;
            }

            // Create a new one if the request contain the data for a new one.
            if (requestDto.ThumbnailFile != null)
            {
                ServiceResult<string> photoServiceResult;
                photoServiceResult = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    "posts",
                    true);
                post.ThumbnailUrl = photoServiceResult.ResponseDto;
            }
        }

        // Update title if needed.
        if (post.Title != requestDto.Title)
        {
            post.Title = requestDto.Title;
            post.NormalizedTitle = await GenerateNormalizedTitle(requestDto.Title);
        }
        // Update other properties
        post.Content = requestDto.Content;
        post.IsPinned = requestDto.IsPinned;
        post.IsPublished = requestDto.IsPublished;
        post.UpdatedDateTime = DateTime.Now;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the detail of the updated entity
        PostDetailResponseDto responseDto = new PostDetailResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            NormalizedTitle = post.NormalizedTitle,
            ThumbnailUrl = post.ThumbnailUrl,
            Content = post.Content,
            CreatedDateTime = post.CreatedDateTime,
            UpdatedDateTime = post.UpdatedDateTime,
            IsPinned = post.IsPinned,
            IsPublished = post.IsPublished,
            Views = post.Views,
            User = new UserBasicResponseDto
            {
                Id = post.User.Id,
                UserName = post.User.UserName
            }
        };
        return ServiceResult<PostDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Delete a post with given id.
    /// </summary>
    /// <param name="id">The id of the post to be deleted.</param>
    /// <returns>The id of the deleted post.</returns>
    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database.
        Post post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);

        // Ensure the entity exists in the database.
        if (post == null)
        {
            return ServiceResult<int>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Post),
                    nameof(id),
                    id.ToString()));
        }

        // Delete the thumbnail file if the exists.
        if (post.ThumbnailUrl != null)
        {
            _photoService.Delete(post.ThumbnailUrl);
        }

        // Delete the entity.
        _context.Posts.Remove(post);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the id of the deleted entity.
        return ServiceResult<int>.Success(post.Id);
    }

    /// <summary>
    /// Generate a unique normalized title for URI which all of its Vietnamese diacritics
    /// have been removed and all of its space characters have been replaced.
    /// </summary>
    /// <param name="title">The title which may contains Vietnamese diacritics.</param>
    /// <returns>The normalized title.</returns>
    private async Task<string> GenerateNormalizedTitle(string title)
    {
        // Determine the normalized title that is not duplicated.
        string originalNormalizedTitle = title
            .ToNonDiacritics()
            .ToLower()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("?", "")
            .Replace("-", "")
            .Replace(":", "")
            .Replace(";", "")
            .Replace("/", "")
            .Replace(">", "")
            .Replace("<", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("đ", "d");
        string normalizedTitle = originalNormalizedTitle;
        List<string> sameNormalizedTitles = await _context.Posts
            .Where(p => p.Title.Contains(originalNormalizedTitle))
            .Select(p => p.Title)
            .ToListAsync();
        Random random = new Random();
        while (sameNormalizedTitles.Contains(normalizedTitle)) {
            normalizedTitle = originalNormalizedTitle + "-" + random.Next(1, 100).ToString();
        }
        return normalizedTitle;
    }
}
