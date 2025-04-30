namespace NATS.Services;

/// <inheritdoc cref="ICertificateService" />
public class CertificateService
    :
        AbstractUpsertableService<Certificate, CertificateUpsertRequestDto>,
        ICertificateService
{
    public CertificateService(DatabaseContext context) : base(context)
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
            .Where(c => c.Id == id)
            .Select(certificate => new CertificateResponseDto(certificate))
            .SingleOrDefaultAsync()
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
            Name = requestDto.Name,
            ThumbnailUrl = requestDto.ThumbnailUrl
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

        certificate.Name = requestDto.Name;
        certificate.ThumbnailUrl = requestDto.ThumbnailUrl;

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