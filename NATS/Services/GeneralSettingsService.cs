namespace NATS.Services;

public class GeneralSettingsService : IGeneralSettingsService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<GeneralSettingsRequestDto> _validator;

    public GeneralSettingsService(
            DatabaseContext context,
            IValidator<GeneralSettingsRequestDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ServiceResult<GeneralSettingsResponseDto>> GetAsync()
    {
        GeneralSettingsResponseDto responseDto = await _context.GeneralSettings
            .Select(gs => new GeneralSettingsResponseDto
            {
                ApplicationName = gs.ApplicationName,
                ApplicationShortName = gs.ApplicationShortName,
                FavIconUrl = gs.FavIconUrl,
                UnderMaintainance = gs.UnderMaintainance
            }).SingleAsync();
        return ServiceResult<GeneralSettingsResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<GeneralSettingsResponseDto>> UpdateAsync(
            GeneralSettingsRequestDto requestDto)
    {
        // Validate data from request
        requestDto = requestDto.TransformValues();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            return ServiceResult<GeneralSettingsResponseDto>.Failed(validationResult.Errors);
        }

        // Fetching for the entity
        GeneralSettings settings = await _context.GeneralSettings.SingleAsync();

        settings.ApplicationName = requestDto.ApplicationName;
        settings.ApplicationShortName = requestDto.ApplicationShortName;
        settings.UnderMaintainance = requestDto.UnderMaintainance;
        await _context.SaveChangesAsync();

        return ServiceResult<GeneralSettingsResponseDto>.Success(new GeneralSettingsResponseDto
        {
            ApplicationName = settings.ApplicationName,
            ApplicationShortName = settings.ApplicationShortName,
            FavIconUrl = settings.FavIconUrl,
            UnderMaintainance = settings.UnderMaintainance
        });
    }
}
