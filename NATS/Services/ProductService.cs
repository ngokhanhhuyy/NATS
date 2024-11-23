namespace NATS.Services;

public class ProductService : IProductService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<ProductRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public ProductService(
            DatabaseContext context,
            IValidator<ProductRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    /// <summary>
    /// Get a list of products with basic information and thumbnail only.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<ProductBasicResponseDto>>> GetBasicListAsync()
    {
        List<ProductBasicResponseDto> responseDtos = await _context.Products
            .OrderBy(c => c.Id)
            .Select(c => new ProductBasicResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Summary = c.Summary,
                ThumbnailUrl = c.ThumbnailUrl
            }).ToListAsync();
        return ServiceResult<List<ProductBasicResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get a list of products with detail information,
    /// containing the thumbnail url, all features and photos.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<ProductDetailResponseDto>>> GetDetailListAsync()
    {
        // Fetch a list of detail entities in the database
        List<ProductDetailResponseDto> responseDtos = await _context.Products
            .Include(c => c.Features)
            .Include(c => c.Photos)
            .OrderBy(c => c.Id)
            .Select(c => new ProductDetailResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Summary = c.Summary,
                Detail = c.Detail,
                ThumbnailUrl = c.ThumbnailUrl,
                Features = c.Features
                    .OrderBy(cs => cs.Id)
                    .Select(cs => new ProductFeatureResponseDto
                    {
                        Id = cs.Id,
                        Content = cs.Content
                    }).ToList(),
                Photos = c.Photos
                    .OrderBy(cp => cp.Id)
                    .Select(cp => new ProductPhotoResponseDto
                    {
                        Id = cp.Id,
                        Url = cp.Url
                    }).ToList()
            })
            .ToListAsync();
        return ServiceResult<List<ProductDetailResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get product by given id with detail information,
    /// containing the thumbnail url, all features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the product
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the product if the product with given id
    /// exists. Otherwise, a ServiceResult object which contains NotFound error will be returned.
    /// </returns>
    public async Task<ServiceResult<ProductDetailResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id from the database
        Product product = await _context.Products
            .Include(c => c.Features)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity exists
        if (product == null)
        {
            return ServiceResult<ProductDetailResponseDto>.Failed(ServiceError.NotFoundByProperty(
                nameof(Product),
                nameof(id),
                id.ToString()));
        }

        // Return the data of the entity
        ProductDetailResponseDto responseDto = new ProductDetailResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Detail = product.Detail,
            ThumbnailUrl = product.ThumbnailUrl,
            Features = product.Features
                .OrderBy(section => section.Id)
                .Select(cs => new ProductFeatureResponseDto
                {
                    Id = cs.Id,
                    Content = cs.Content
                }).ToList(),
            Photos = product.Photos
                .OrderBy(photo => photo.Id)
                .Select(cp => new ProductPhotoResponseDto
                {
                    Id = cp.Id,
                    Url = cp.Url
                }).ToList()
        };
        return ServiceResult<ProductDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Create a product by the data sent from the request, containing thumbnail file, features and photos.
    /// </summary>
    /// <param name="requestDto">
    /// An object that contains all data for a new product sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the created product if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<ProductDetailResponseDto>> CreateAsync(ProductRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Create")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<ProductDetailResponseDto>.Failed(result.Errors);
        }

        Product product = new Product
        {
            Name = requestDto.Name,
            Summary = requestDto.Summary,
            Detail = requestDto.Detail,
            Features = new List<ProductFeature>(),
            Photos = new List<ProductPhoto>()
        };

        // Create new thumbnail if the request contains it
        if (requestDto.ThumbnailFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "products",
                true);
            product.ThumbnailUrl = photoServiceResult.ResponseDto;
        }

        // Create features
        if (requestDto.Features != null)
        {
            foreach (ProductFeatureRequestDto featureRequestDto in requestDto.Features)
            {
                ProductFeature feature = new ProductFeature
                {
                    Content = featureRequestDto.Content
                };
                product.Features.Add(feature);
            }
        }

        // Create photos
        if (requestDto.Photos != null)
        {
            foreach (ProductPhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                ServiceResult<string> photoServiceResult;
                photoServiceResult = await _photoService.CreateAsync(
                    photoRequestDto.File,
                    "products",
                    false);
                ProductPhoto photo = new ProductPhoto
                {
                    Url = photoServiceResult.ResponseDto
                };
                product.Photos.Add(photo);
            }
        }

        // Save changes
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Return the data of the created entity
        ProductDetailResponseDto responseDto = new ProductDetailResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Detail = product.Detail,
            ThumbnailUrl = product.ThumbnailUrl,
            Features = product.Features
                .OrderBy(section => section.Id)
                .Select(section => new ProductFeatureResponseDto
                {
                    Id = section.Id,
                    Content = section.Content
                }).ToList(),
            Photos = product.Photos
                .OrderBy(photo => photo.Id)
                .Select(photo => new ProductPhotoResponseDto
                {
                    Id = photo.Id,
                    Url = photo.Url
                }).ToList()
        };
        return ServiceResult<ProductDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update the product which has the given id with the data sent from the request,
    /// containing thumbnail file, features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the product to be updated
    /// </param>
    /// <param name="requestDto">
    /// An object that contains all data for a new product sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the updated product if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<ProductDetailResponseDto>> UpdateAsync(
            int id,
            ProductRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Update")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<ProductDetailResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity in the database
        Product product = await _context.Products
            .Include(c => c.Features)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity with given id exists in the database
        if (product == null)
        {
            return ServiceResult<ProductDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Product),
                    nameof(id),
                    id.ToString()));
        }

        // Replace the thumbnail with a new one if it has been changed
        if (requestDto.ThumbnailChanged)
        {
            // Delete the old thumbnail with the URL stored in the entity property if exists
            if (product.ThumbnailUrl != null)
            {
                _photoService.Delete(product.ThumbnailUrl);
                product.ThumbnailUrl = null;
            }
            // Add a new thumbnail if the request contains it
            if (requestDto.ThumbnailFile != null)
            {
                ServiceResult<string> photoServiceResult = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    "products",
                    true);
                product.ThumbnailUrl = photoServiceResult.ResponseDto;
            }
        }

        product.Name = requestDto.Name;
        product.Summary = requestDto.Summary;
        product.Detail = requestDto.Detail;

        // Update sections
        if (requestDto.Features != null)
        {
            foreach (ProductFeatureRequestDto featureRequestDto in requestDto.Features)
            {
                // Perform updating operation if this section has id value.
                // Otherwise, perform creating operation.
                ProductFeature feature;
                if (featureRequestDto.Id.HasValue)
                {
                    feature = product.Features
                        .SingleOrDefault(s => s.Id == featureRequestDto.Id);
                    // Ensure the entity with given id exists.
                    if (feature == null)
                    {
                        return ServiceResult<ProductDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(featureRequestDto.Id)));
                    }

                    // Perform delete operation if indicated. Otherwise, update it's content.
                    if (featureRequestDto.IsDeleted)
                    {
                        _context.ProductFeatures.Remove(feature);
                    }
                    else
                    {
                        feature.Content = featureRequestDto.Content;
                    }
                }
                else
                {
                    feature = new ProductFeature
                    {
                        Content = featureRequestDto.Content,
                        ProductId = product.Id
                    };
                    _context.ProductFeatures.Add(feature);
                }
            }
        }

        // Update photos
        if (requestDto.Photos != null)
        {
            foreach (ProductPhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                ProductPhoto photo;
                ServiceResult<string> photoServiceResult;
                // Perform updating operation if this photo has id value.
                if (photoRequestDto.Id.HasValue)
                {
                    photo = product.Photos
                        .SingleOrDefault(p => p.Id == photoRequestDto.Id);

                    // Ensure the photo with given id exists
                    if (photo == null)
                    {
                        return ServiceResult<ProductDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(photoRequestDto.Id)));
                    }

                    // Delete the old photo having URL stored in the entity property
                    if (photoRequestDto.IsDeleted)
                    {
                        // Delete old photo in filesystem and the entity from the database
                        _photoService.Delete(photo.Url);
                        _context.ProductPhotos.Remove(photo);
                    }
                }
                // Otherwise, perform creating operation.
                else if (!photoRequestDto.IsDeleted)
                {
                    photoServiceResult = await _photoService.CreateAsync(
                        photoRequestDto.File,
                        "products",
                        false);
                    photo = new ProductPhoto
                    {
                        Url = photoServiceResult.ResponseDto,
                        ProductId = product.Id
                    };
                    _context.ProductPhotos.Add(photo);
                }
            }
        }

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity
        ProductDetailResponseDto responseDto = new ProductDetailResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Detail = product.Detail,
            ThumbnailUrl = product.ThumbnailUrl,
            Features = product.Features
                .OrderBy(section => section.Id)
                .Select(s => new ProductFeatureResponseDto
                {
                    Id = s.Id,
                    Content = s.Content
                }).ToList(),
            Photos = product.Photos
                .OrderBy(photo => photo.Id)
                .Select(p => new ProductPhotoResponseDto
                {
                    Id = p.Id,
                    Url = p.Url
                }).ToList()
        };
        return ServiceResult<ProductDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Delete a product which has the given id, including its thumbnail, features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the product to be deleted
    /// </param>
    /// <returns>
    /// The id of the product which has been deleted if the operation was successful.
    /// Otherwise, a ServiceResult object which contains NotFound error.
    /// </returns>
    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database
        Product product = await _context.Products
            .Include(c => c.Features)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity with given id exists in the database
        if (product == null)
        {
            return ServiceResult<int>.Failed(ServiceError.NotFoundByProperty(
                nameof(Course),
                nameof(id),
                id.ToString()
            ));
        }

        // Delete all features
        foreach (ProductFeature feature in product.Features)
        {
            _context.ProductFeatures.Remove(feature);
        }

        // Delete all photos
        foreach (ProductPhoto photo in product.Photos)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = _photoService.Delete(photo.Url);
            _context.ProductPhotos.Remove(photo);
        }

        // Delete the entity
        _context.Products.Remove(product);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the id of the deleted entity
        return ServiceResult<int>.Success(product.Id);
    }
}