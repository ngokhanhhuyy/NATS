namespace NATS.Services;

public class IntroductionItemService : IIntroductionItemService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<IntroductionItemRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public IntroductionItemService(
            DatabaseContext context,
            IValidator<IntroductionItemRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    /// <summary>
    /// Get a list of introduction items.
    /// </summary>
    /// <param name="itemType">The type of introduction items.</param>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<IntroductionItemResponseDto>>> GetListAsync()
    {
        List<IntroductionItemResponseDto> responseDtos;
        responseDtos = await _context.IntroductionItems
            .OrderBy(ii => ii.Id)
            .Take(4)
            .Select(ii => new IntroductionItemResponseDto
            {
                Id = ii.Id,
                Name = ii.Name,
                Summary = ii.Summary,
                Content = ii.Content,
                ThumbnailUrl = ii.ThumbnailUrl
            }).ToListAsync();
        return ServiceResult<List<IntroductionItemResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get the introduction item with given id and item type.
    /// </summary>
    /// <param name="id">The id of the introduction item.</param>
    /// <param name="itemType">The type of the introduction item.</param>
    /// <returns>
    /// ServiceResult object that contain the data of the product if the product with given id
    /// exists. Otherwise, a ServiceResult object which contains NotFound error will be returned.
    /// </returns>
    public async Task<ServiceResult<IntroductionItemResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id in the database
        IntroductionItem introductionItem;
        introductionItem = await _context.IntroductionItems
            .SingleOrDefaultAsync(ii => ii.Id == id);

        // Ensure the entity exists
        if (introductionItem == null)
        {
            return ServiceResult<IntroductionItemResponseDto>.Failed(ServiceError.NotFoundByProperty(
                nameof(IntroductionItem),
                nameof(id),
                id.ToString()));
        }

        // Return data of the entity
        IntroductionItemResponseDto responseDto = new IntroductionItemResponseDto
        {
            Id = introductionItem.Id,
            Name = introductionItem.Name,
            Summary = introductionItem.Summary,
            Content = introductionItem.Content,
            ThumbnailUrl = introductionItem.ThumbnailUrl
        };
        return ServiceResult<IntroductionItemResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update the introduction item with given id, item type and request data.
    /// </summary>
    /// <param name="id">The id of the introduction item.</param>
    /// <param name="itemType">The type of the introduction item.</param>
    /// <param name="requestDto">The object that contains the new data for the introduction item.</param>
    /// <returns>
    /// ServiceResult object that contain the data of the updated introduction item if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<IntroductionItemResponseDto>> UpdateAsync(
            int id,
            IntroductionItemRequestDto requestDto)
    {
        // Validate data from the request
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<IntroductionItemResponseDto>.Failed(result.Errors);
        }

        // Fetch for the entity from the database
        IntroductionItem item = await _context.IntroductionItems
            .SingleOrDefaultAsync(ii => ii.Id == id);

        // Ensure the entity exists in the database
        if (item == null)
        {
            return ServiceResult<IntroductionItemResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(IntroductionItem),
                    nameof(id),
                    id.ToString()));
        }

        // Replace the thumbnail if the thumbnail has been changed
        if (requestDto.ThumbnailChanged)
        {
            // Delete the old thumbnail
            _photoService.Delete(item.ThumbnailUrl);

            // Create a new one
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "introduction-items",
                true);
            // Update the thumbnail URL of the entity
            item.ThumbnailUrl = photoServiceResult.ResponseDto;
        }
        // Update other properties
        item.Name = requestDto.Name;
        item.Summary = requestDto.Summary;
        item.Content = requestDto.Content;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity
        IntroductionItemResponseDto responseDto = new IntroductionItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Summary = item.Summary,
            Content = item.Content,
            ThumbnailUrl = item.ThumbnailUrl
        };
        return ServiceResult<IntroductionItemResponseDto>.Success(responseDto);
    }
}