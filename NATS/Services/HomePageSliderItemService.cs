namespace NATS.Services;

public class HomePageSliderItemService : IHomePageSliderItemService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService _photoService;
    private readonly IValidator<SliderItem> _validator;

    public HomePageSliderItemService(
            DatabaseContext context,
            IPhotoService photoService,
            IValidator<SliderItem> validator)
    {
        _context = context;
        _photoService = photoService;
        _validator = validator;
    }

    /// <summary>
    /// Get a list of all homepage slider items.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<SliderItemResponseDto>>> GetListAsync()
    {
        List<SliderItemResponseDto> responseDtos;
        responseDtos = await _context.SliderItems
            .OrderBy(i => i.Index)
            .Select(i => new SliderItemResponseDto
            {
                Id = i.Id,
                Title = i.Title,
                PhotoUrl = i.PhotoUrl,
                Index = i.Index
            }).ToListAsync();
        return ServiceResult<List<SliderItemResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get the homepage slider item with given id.
    /// </summary>
    /// <param name="id">The id of the homepage slider item.</param>
    /// <returns>
    /// ServiceResult object that contains the data of the result.
    /// </returns>
    public async Task<ServiceResult<SliderItemResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id from the database.
        SliderItem item = await _context.SliderItems
            .SingleOrDefaultAsync(i => i.Id == id);

        // Ensure the entity exists in the database.
        if (item == null)
        {
            return ServiceResult<SliderItemResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(SliderItem),
                    nameof(id),
                    id.ToString()));
        }

        // Map the entity properties to the response dto.
        SliderItemResponseDto responseDto;
        responseDto = new SliderItemResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            PhotoUrl = item.PhotoUrl,
            Index = item.Index
        };

        // Return the response dto
        return ServiceResult<SliderItemResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Create a new homepage slider item with the data from the request.
    /// </summary>
    /// <param name="upsertRequestDto">The object contains the data for a new homepage slider item.</param>
    /// <returns>
    /// ServiceResult object that contains the data of the result.
    /// </returns>
    public async Task<ServiceResult<SliderItemResponseDto>> CreateAsync(
            SliderItem upsertRequestDto)
    {
        // Validate data from the request
        ValidationResult result = _validator.Validate(
            upsertRequestDto.TransformValues(),
            options => options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<SliderItemResponseDto>.Failed(result.Errors);
        }

        // Index of the new item provided in the request will be ignored,
        // might be developed further in the future.

        // Fetch the biggest index in the database.
        int biggestIndex = await _context.SliderItems.MaxAsync(i => i.Index);

        // Create new photo
        ServiceResult<string> photoServiceResult;
        photoServiceResult = await _photoService.CreateAsync(
            upsertRequestDto.PhotoFile,
            "homepage-slider-items",
            (double)1200 / 550);

        // Initialize a new entity
        SliderItem item = new SliderItem
        {
            Title = upsertRequestDto.Title,
            PhotoUrl = photoServiceResult.ResponseDto,
            Index = biggestIndex + 1
        };
        _context.SliderItems.Add(item);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the detail of the created entity
        SliderItemResponseDto responseDto = new SliderItemResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            PhotoUrl = item.PhotoUrl,
            Index = item.Index
        };
        return ServiceResult<SliderItemResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update the homepage slider item with given id and the data from the request.
    /// </summary>
    /// <param name="id">The id of the homepage slider item to be updated.</param>
    /// <param name="upsertRequestDto">The object contains the new data to be updated.</param>
    /// <returns>
    /// ServiceResult object that contains the data of the result.
    /// </returns>
    public async Task<ServiceResult<SliderItemResponseDto>> UpdateAsync(
            int id,
            SliderItem upsertRequestDto)
    {
        // Validate the data from the request
        ValidationResult result = _validator.Validate(
            upsertRequestDto.TransformValues(),
            options => options.IncludeRuleSets("Update")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<SliderItemResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity with given id from the database
        SliderItem item = await _context.SliderItems
            .SingleOrDefaultAsync(i => i.Id == id);
        
        // Ensure the entity exists in the database
        if (item == null)
        {
            return ServiceResult<SliderItemResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(SliderItem),
                    nameof(id),
                    id.ToString()));
        }

        // Create a new photo if the request provided the data for a new one.
        if (upsertRequestDto.PhotoChanged)
        {
            // Remove the old photo
            _photoService.Delete(item.PhotoUrl);
            // Create a new one
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                upsertRequestDto.PhotoFile,
                "homepage-slider-items",
                (double)1200 / 550);
            item.PhotoUrl = photoServiceResult.ResponseDto;
        }

        // Update the entity's properties
        item.Title = upsertRequestDto.Title;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the updated detail of the entity
        SliderItemResponseDto responseDto = new SliderItemResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            PhotoUrl = item.PhotoUrl,
            Index = item.Index
        };
        return ServiceResult<SliderItemResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Delete the homepage slider item with given id.
    /// </summary>
    /// <param name="id">The id of the homepage slider item to be deleted.</param>
    /// <returns>
    /// The id of the deleted homepage slider item.
    /// </returns>
    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database.
        SliderItem item = await _context.SliderItems
            .SingleOrDefaultAsync(i => i.Id == id);
        
        // Ensure the entity exists in the database.
        if (item == null)
        {
            return ServiceResult<int>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(SliderItem),
                    nameof(id),
                    id.ToString()));
        }

        // Delete the entity.
        _context.SliderItems.Remove(item);

        // Save changes.
        await _context.SaveChangesAsync();

        // Return the id if the deleted entity.
        return ServiceResult<int>.Success(item.Id);
    }
}