namespace NATS.Services;

/// <inheritdoc cref="IGeneralSettingsService" />
public class GeneralSettingsService
    :
        AbstractUpsertableService<GeneralSettings, GeneralSettingsUpsertRequestDto>,
        IGeneralSettingsService
{
    /// <inheritdoc />
    public GeneralSettingsService(DatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<GeneralSettingsResponseDto> GetAsync()
    {
        return await Context.GeneralSettings
            .Select(gs => new GeneralSettingsResponseDto(gs))
            .SingleAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(GeneralSettingsUpsertRequestDto requestDto)
    {
        // Fetching the entity from the database.
        GeneralSettings settings = await Context.GeneralSettings.SingleAsync();

        // Update the entity's properties.
        settings.ApplicationName = requestDto.ApplicationName;
        settings.ApplicationShortName = requestDto.ApplicationShortName;
        settings.UnderMaintainance = requestDto.UnderMaintainance;

        // Save changes.
        await base.SaveUpdatedEntityAsync(settings, requestDto);
    }

    /// <inheritdoc />
    protected sealed override DbSet<GeneralSettings> GetRepository(DatabaseContext context)
    {
        return context.GeneralSettings;
    }
}
