namespace NATS.Services;

/// <inheritdoc />
public class CertificateService : ICertificateService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService _photoService;

    public CertificateService(DatabaseContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    /// <inheritdoc />
    public async Task<List<CertificateResponseDto>> GetListAsync()
    {
        return await _context.Certificates
            .OrderBy(certificate => certificate.Id)
            .Select(certificate => new CertificateResponseDto(certificate))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<CertificateResponseDto> GetSingleAsync(int id)
    {
        return await _context.Certificates
            .Select(certificate => new CertificateResponseDto(certificate))
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CertificateUpsertRequestDto upsertRequestDto)
    {
        // Initialize a new entity.
        Certificate certificate = new Certificate
        {
            Name = upsertRequestDto.Name
        };
        
        _context.Certificates.Add(certificate);
        
        // Save the photo if exists.
        if (upsertRequestDto.PhotoFile != null)
        {
            certificate.PhotoUrl = await _photoService.CreateAsync(
                upsertRequestDto.PhotoFile,
                "certificates");
        }

        try
        {
            await _context.SaveChangesAsync();
            return certificate.Id;
        }
        catch
        {
            // Remove the created photo if the operation fails.
            _photoService.Delete(certificate.PhotoUrl);

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, CertificateUpsertRequestDto upsertRequestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Certificate certificate = await _context.Certificates
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());

        // Update photo if changed.
        string urlToBeDeletedWhenFailure = null;
        string urlToBeDeletedWhenSuccess = null;
        if (upsertRequestDto.PhotoChanged)
        {
            // Delete old photo if exists
            if (certificate.PhotoUrl != null)
            {
                urlToBeDeletedWhenSuccess = certificate.PhotoUrl;
                certificate.PhotoUrl = null;
            }
            
            // Create new photo if it's data is included in the request
            if (upsertRequestDto.PhotoFile != null)
            {
                 certificate.PhotoUrl = await _photoService.CreateAsync(
                     upsertRequestDto.PhotoFile,
                     "certificates");
                 urlToBeDeletedWhenFailure = certificate.PhotoUrl;
            }
        }

        // Update the entity's other property.
        certificate.Name = upsertRequestDto.Name;

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();

            // The operation is successful, delete the old photo file.
            if (urlToBeDeletedWhenSuccess != null)
            {
                _photoService.Delete(urlToBeDeletedWhenSuccess);
            }
        }
        catch (DbUpdateException exception)
        {
            // The operation is failed, delete the created photo.
            if (urlToBeDeletedWhenFailure != null)
            {
                _photoService.Delete(urlToBeDeletedWhenFailure);
            }
            
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity and ensure it exists.
        Certificate certificate = await _context.Certificates
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());
        
        // Delete the entity from the database.
        _context.Certificates.Remove(certificate);

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();
            
            // The operation is successful, delete the photo file.
            if (certificate.PhotoUrl != null)
            {
                _photoService.Delete(certificate.PhotoUrl);
            }
        }
        catch (DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
}