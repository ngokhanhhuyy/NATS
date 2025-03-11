namespace NATS.Services;

/// <inheritdoc cref="ICertificateService" />
public class CertificateService
    :
        AbstractHasThumbnailService<Certificate, CertificateUpsertRequestDto>,
        ICertificateService
{
    public CertificateService(DatabaseContext context, IPhotoService photoService)
            : base(context, photoService)
    {
    }

    /// <inheritdoc />
    public async Task<List<CertificateResponseDto>> GetListAsync()
    {
        return await Context.Certificates
            .OrderBy(certificate => certificate.Id)
            .Select(certificate => new CertificateResponseDto(certificate))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<CertificateResponseDto> GetSingleAsync(int id)
    {
        return await Context.Certificates
            .Select(certificate => new CertificateResponseDto(certificate))
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CertificateUpsertRequestDto requestDto)
    {
        Certificate certificate = new Certificate
        {
            Name = requestDto.Name
        };

        return await base.SaveCreatedEntityAsync(certificate, requestDto);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, CertificateUpsertRequestDto requestDto)
    {
        Certificate certificate = await Context.Certificates
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());

        await base.SaveUpdatedEntityAsync(certificate, requestDto);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        Certificate certificate = await Context.Certificates
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Certificate),
                nameof(id),
                id.ToString());

        await base.SaveDeletedEntityAsync(certificate);
    }

    /// <inheritdoc />
    protected override sealed DbSet<Certificate> GetRepository(DatabaseContext context)
    {
        return context.Certificates;
    }
}