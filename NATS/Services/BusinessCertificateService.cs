using NATS.Services.Entity;

namespace NATS.Services;

public class BusinessCertificateService : IBusinessCertificateService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<BusinessCertificateRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public BusinessCertificateService(
            DatabaseContext context,
            IValidator<BusinessCertificateRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    public async Task<ServiceResult<List<BusinessCertificateResponseDto>>> GetListAsync()
    {
        List<BusinessCertificateResponseDto> responseDtos;
        responseDtos = await _context.BusinessCertificates
            .OrderBy(bc => bc.Id)
            .Select(bc => new BusinessCertificateResponseDto
            {
                Id = bc.Id,
                Name = bc.Name,
                PhotoUrl = bc.PhotoUrl
            }).ToListAsync();
        return ServiceResult<List<BusinessCertificateResponseDto>>.Success(responseDtos);
    }

    public async Task<ServiceResult<BusinessCertificateResponseDto>> GetAsync(int id)
    {
        // Fetch for the entity by id
        BusinessCertificate certificate;
        certificate = await _context.BusinessCertificates
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity exists in the database
        if (certificate == null)
        {
            return ServiceResult<BusinessCertificateResponseDto>.Failed(
                ServiceError.NotAvailableByProperty(
                    nameof(BusinessCertificate),
                    nameof(id),
                    id.ToString()));
        }

        BusinessCertificateResponseDto responseDto = new BusinessCertificateResponseDto
        {
            Id = certificate.Id,
            Name = certificate.Name,
            PhotoUrl = certificate.PhotoUrl
        };
        return ServiceResult<BusinessCertificateResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<BusinessCertificateResponseDto>> CreateAsync(
            BusinessCertificateRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto,
            options => options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<BusinessCertificateResponseDto>.Failed(result.Errors);
        }

        // Save the photo if exists
        string photoUrl = null;
        if (requestDto.PhotoFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.PhotoFile,
                "certificates",
                false);
            photoUrl = photoServiceResult.ResponseDto;
        }

        // Initialize business certificate entity
        BusinessCertificate certificate = new BusinessCertificate
        {
            Name = requestDto.Name,
            PhotoUrl = photoUrl
        };
        _context.BusinessCertificates.Add(certificate);
        await _context.SaveChangesAsync();

        // Returning the data of the created entity
        BusinessCertificateResponseDto responseDto;
        responseDto = new BusinessCertificateResponseDto
        {
            Id = certificate.Id,
            Name = certificate.Name
        };
        return ServiceResult<BusinessCertificateResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<BusinessCertificateResponseDto>> UpdateAsync(
            int id,
            BusinessCertificateRequestDto requestDto)
    {
        // Validate data from the request
        ValidationResult result = _validator.Validate(
            requestDto,
            options => options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<BusinessCertificateResponseDto>.Failed(result.Errors);
        }

        // Ensure the entity exists in the database
        BusinessCertificate certificate;
        certificate = await _context.BusinessCertificates.SingleOrDefaultAsync(c => c.Id == id);
        if (certificate == null)
        {
            return ServiceResult<BusinessCertificateResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(BusinessCertificate),
                    nameof(id),
                    id.ToString()));
        }

        // Update photo
        if (requestDto.PhotoChanged)
        {
            ServiceResult<string> photoServiceResult;
            // Delete old photo if exists
            if (certificate.PhotoUrl != null)
            {
                photoServiceResult = _photoService.Delete(certificate.PhotoUrl);
                certificate.PhotoUrl = null;
            }
            // Create new photo if it's data is included in the request
            if (requestDto.PhotoFile != null)
            {
                photoServiceResult = await _photoService
                    .CreateAsync(requestDto.PhotoFile, "certificates", false);
                certificate.PhotoUrl = photoServiceResult.ResponseDto;
            }
        }

        // Update business certificate entity's column
        certificate.Name = requestDto.Name;

        // Save changes
        await _context.SaveChangesAsync();

        // Return data of the updated entity
        BusinessCertificateResponseDto responseDto = new BusinessCertificateResponseDto
        {
            Id = certificate.Id,
            Name = certificate.Name,
            PhotoUrl = certificate.PhotoUrl
        };
        return ServiceResult<BusinessCertificateResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Ensure the entity exists in the database
        BusinessCertificate certificate;
        certificate = await _context.BusinessCertificates
            .SingleOrDefaultAsync(c => c.Id == id);
        if (certificate == null)
        {
            return ServiceResult<int>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(BusinessCertificate),
                    nameof(id),
                    id.ToString()));
        }
        // Delete the photo of the entity in the filesystem
        _photoService.Delete(certificate.PhotoUrl);
        
        // Delete the entity from the database
        _context.BusinessCertificates.Remove(certificate);

        // Save changes
        await _context.SaveChangesAsync();

        // Returning the id of the deleted entity
        return ServiceResult<int>.Success(id);
    }
}