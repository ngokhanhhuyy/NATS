namespace NATS.Services;

public class AboutUsIntroductionService : IAboutUsIntroductionService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<AboutUsIntroductionRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public AboutUsIntroductionService(
            DatabaseContext context,
            IValidator<AboutUsIntroductionRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    public async Task<AboutUsIntroductionResponseDto> GetAsync()
    {
        return await _context.AboutUsIntroductions
            .Select(aui => new AboutUsIntroductionResponseDto(aui))
            .SingleAsync();
    }

    public async Task<AboutUsIntroductionResponseDto> UpdateAsync(
            AboutUsIntroductionRequestDto requestDto)
    {
        // Fetch the entity from the database.
        AboutUsIntroduction introduction = await _context.AboutUsIntroductions.SingleAsync();

        // Update photo when the request indicates that it has been changed.
        string photoUrlToBeDeletedWhenSuccess = null;
        string photoUrlToBeDeletedWhenFailure = null;
        if (requestDto.MainPhotoChanged)
        {
            // Delete the old photo if exists
            if (introduction.ThumbnailUrl != null)
            {
                _photoService.Delete(introduction.ThumbnailUrl);
                introduction.ThumbnailUrl = null;
            }

            // Create a new photo if the request contains the data for a new one.
            if (requestDto.MainPhotoFile != null)
            {
                introduction.ThumbnailUrl = await _photoService.CreateAsync(
                    requestDto.MainPhotoFile,
                    "about-us",
                    false);
            }
        }

        // Update the other properties.
        introduction.MainQuoteContent = requestDto.MainQuoteContent;
        introduction.AboutUsContent = requestDto.AboutUsContent;
        introduction.WhyChooseUsContent = requestDto.WhyChooseUsContent;
        introduction.OurDifferenceContent = requestDto.OurDifferenceContent;
        introduction.OurCultureContent = requestDto.OurCultureContent;

        // Save changes
        try
        {
            await _context.SaveChangesAsync();
        }

        // Return the data of the updated entity
        return ServiceResult<AboutUsIntroductionResponseDto>.Success(
            new AboutUsIntroductionResponseDto
            {
                ThumbnailUrl = introduction.ThumbnailUrl,
                MainQuoteContent = introduction.MainQuoteContent,
                AboutUsContent = introduction.AboutUsContent,
                WhyChooseUsContent = introduction.WhyChooseUsContent,
                OurDifferenceContent = introduction.OurDifferenceContent,
                OurCultureContent = introduction.OurCultureContent
            });
    }
}