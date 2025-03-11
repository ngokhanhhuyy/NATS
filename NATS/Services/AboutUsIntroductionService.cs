namespace NATS.Services;

public class AboutUsIntroductionService
        :
            AbstractHasThumbnailService<
                AboutUsIntroduction,
                AboutUsIntroductionUpsertRequestDto>,
            IAboutUsIntroductionService
{
    public AboutUsIntroductionService(
            DatabaseContext context,
            IPhotoService photoService) : base(context, photoService)
    {
    }

    public async Task<AboutUsIntroductionResponseDto> GetAsync()
    {
        return await Context.AboutUsIntroductions
            .Select(aui => new AboutUsIntroductionResponseDto(aui))
            .SingleAsync();
    }

    public async Task UpdateAsync(AboutUsIntroductionUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database.
        AboutUsIntroduction introduction = await Context.AboutUsIntroductions.SingleAsync();

        // Update the entity's properties.
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