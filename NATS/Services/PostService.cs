using System.Text.RegularExpressions;

namespace NATS.Services;

/// <inheritdoc cref="IPostService" />
public partial class PostService
    : 
        AbstractUpsertableService<Post, PostUpsertRequestDto>,
        IPostService
{
    private readonly IAuthorizationService _authorizationService;
    
    public PostService(
            DatabaseContext context,
            IAuthorizationService authorizationService) : base(context)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc/>
    public async Task<PostListResponseDto> GetListAsync(int page, int resultsPerPage = 15)
    {
        // Initialize the response dto.
        PostListResponseDto responseDto = new PostListResponseDto();

        // Determine the page count.
        int postCount = await Context.Posts.CountAsync();
        if (postCount == 0)
        {
            responseDto.PageCount = 0;
            return responseDto;
        }
        else
        {
            responseDto.PageCount = (int)Math.Ceiling((double)postCount / resultsPerPage);
        }
        
        responseDto.Results = await Context.Posts
            .OrderBy(post => post.IsPinned)
            .ThenByDescending(post => post.CreatedDateTime)
            .ThenBy(post => post.NormalizedTitle)
            .Select(post => new PostBasicResponseDto(post))
            .Skip((page - 1) * resultsPerPage)
            .Take(resultsPerPage)
            .ToListAsync();
        
        return responseDto;
    }

    /// <inheritdoc/>
    public async Task<PostDetailResponseDto> GetDetailAsync(
            int id,
            bool viewsIncrement = false)
    {
        // Fetch the entity from the database and ensure it exists.
        Post post = await Context.Posts
            .Include(p => p.User)
            .ThenInclude(u => u.Roles)
            .SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new ResourceNotFoundException();
        
        return await HandlePostViewIncrementAndMapToResponseDtoAsync(post, viewsIncrement);
    }
    
    /// <inheritdoc/>
    public async Task<PostDetailResponseDto> GetDetailAsync(
            string normalizedTitle,
            bool viewsIncrement = true)
    {
        // Fetch the entity from the database and ensure it exists.
        Post post = await Context.Posts
            .Include(p => p.User)
            .ThenInclude(u => u.Roles)
            .SingleOrDefaultAsync(p => p.NormalizedTitle == normalizedTitle)
            ?? throw new ResourceNotFoundException();

        return await HandlePostViewIncrementAndMapToResponseDtoAsync(post, viewsIncrement);
    }

    /// <inheritdoc/>
    public async Task<PostStatsResponseDto> GetStatsAsync()
    {
        return await Context.Posts
            .Select(_ => new PostStatsResponseDto
            {
                TotalCount = Context.Posts.Count(),
                TotalViews = Context.Posts.Sum(p => p.Views),
                UnpublishedCount = Context.Posts.Count(p => !p.IsPublished),
            }).Take(1)
            .SingleAsync();
    }
    
    /// <inheritdoc/>
    public async Task<int> CreateAsync(PostUpsertRequestDto requestDto)
    {
        // Initialize a new entity.
        Post post = new Post
        {
            Title = requestDto.Title,
            NormalizedTitle = await GenerateNormalizedTitle(requestDto.Title),
            Content = requestDto.Content,
            IsPinned = requestDto.IsPinned,
            IsPublished = requestDto.IsPublished,
            ThumbnailUrl = requestDto.ThumbnailUrl,
            UserId = (await _authorizationService.GetCallerUserDetailAsync()).Id,
        };

        // Save changes.
        return await base.SaveCreatedEntityAsync(post, requestDto);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, PostUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Post post = await Context.Posts
            .Include(p => p.User)
            .SingleOrDefaultAsync(p => p.Id == id)
            ?? throw GetResourceNotFoundExceptionById(id);


        // Update title if changed.
        if (post.Title != requestDto.Title)
        {
            post.Title = requestDto.Title;
            post.NormalizedTitle = await GenerateNormalizedTitle(requestDto.Title);
        }

        // Update the entity's other properties.
        post.Content = requestDto.Content;
        post.IsPinned = requestDto.IsPinned;
        post.IsPublished = requestDto.IsPublished;
        post.ThumbnailUrl = requestDto.ThumbnailUrl;
        post.UpdatedDateTime = DateTime.Now;

        // Save changes.
        await base.SaveUpdatedEntityAsync(post, requestDto);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        Post post = await Context.Posts
            .Include(p => p.User)
            .SingleOrDefaultAsync(p => p.Id == id)
            ?? throw GetResourceNotFoundExceptionById(id);

        await base.SaveDeletedEntityAsync(post);
    }

    /// <inheritdoc/>
    protected override sealed DbSet<Post> GetRepository(DatabaseContext context)
    {
        return context.Posts;
    }

    /// <summary>
    /// Increments the view property of the specified post entity, saves changes to the
    /// database and handling concurrency exception if occuring.
    /// </summary>
    /// <param name="post">
    /// The post to handle.
    /// </param>
    /// <param name="viewsIncrement">
    /// Indicates whether the post's view should be incremented after the operation finishes.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// mapped DTO containing the detail information of the post.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception> 
    private async Task<PostDetailResponseDto> HandlePostViewIncrementAndMapToResponseDtoAsync(
            Post post,
            bool viewsIncrement)
    {
        // Increment views if specified.
        if (viewsIncrement)
        {
            post.Views += 1;
        }
        
        try
        {
            await Context.SaveChangesAsync();

            return new PostDetailResponseDto(post);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
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
        string originalNormalizedTitle = NormalizedTitleProhibitedCharactersRegex()
            .Replace(
                title.ToNonDiacritics()
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace("đ", "d"),
                "");

        string normalizedTitle = originalNormalizedTitle;
        List<string> sameNormalizedTitles = await Context.Posts
            .Where(p => p.Title.Contains(originalNormalizedTitle))
            .Select(p => p.Title)
            .ToListAsync();
        Random random = new Random();
        while (sameNormalizedTitles.Contains(normalizedTitle)) {
            normalizedTitle = originalNormalizedTitle + "-" + random.Next(1, 100).ToString();
        }
        return normalizedTitle;
    }

    [GeneratedRegex(@"[.,\?\-:;/><\(\)]")]
    private static partial Regex NormalizedTitleProhibitedCharactersRegex();

}
