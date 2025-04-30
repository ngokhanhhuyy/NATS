namespace NATS.Services;

/// <inheritdoc cref="IAboutUsIntroductionService" />
public class AboutUsIntroductionService
        :
            AbstractUpsertableService<
                AboutUsIntroduction,
                AboutUsIntroductionUpdateRequestDto>,
            IAboutUsIntroductionService
{
    public AboutUsIntroductionService(DatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<AboutUsIntroductionResponseDto> GetAsync()
    {
        return await Context.AboutUsIntroductions
            .Select(aui => new AboutUsIntroductionResponseDto(aui))
            .SingleAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(AboutUsIntroductionUpdateRequestDto requestDto)
    {
        // Fetch the entity from the database.
        AboutUsIntroduction introduction = await Context.AboutUsIntroductions.SingleAsync();

        // Update the entity's properties.
        introduction.ThumbnailUrl = requestDto.ThumbnailUrl;
        introduction.MainQuoteContent = requestDto.MainQuoteContent;
        introduction.AboutUsContent = requestDto.AboutUsContent;
        introduction.WhyChooseUsContent = requestDto.WhyChooseUsContent;
        introduction.OurDifferenceContent = requestDto.OurDifferenceContent;
        introduction.OurCultureContent = requestDto.OurCultureContent;

        await base.SaveUpdatedEntityAsync(introduction, requestDto);
    }

    protected override sealed DbSet<AboutUsIntroduction> GetRepository(DatabaseContext context)
    {
        return context.AboutUsIntroductions;
    }
}