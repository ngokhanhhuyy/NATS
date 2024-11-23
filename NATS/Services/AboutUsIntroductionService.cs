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

    public async Task<ServiceResult<AboutUsIntroductionResponseDto>> GetAsync()
    {
        AboutUsIntroductionResponseDto responseDto = await _context.AboutUsIntroductions
            .Select(aui => new AboutUsIntroductionResponseDto
            {
                MainPhotoUrl = aui.MainPhotoUrl,
                MainQuoteContent = aui.MainQuoteContent,
                AboutUsContent = aui.AboutUsContent,
                WhyChooseUsContent = aui.WhyChooseUsContent,
                OurDifferenceContent = aui.OurDifferenceContent,
                OurCultureContent = aui.OurCultureContent
            }).SingleAsync();
        return ServiceResult<AboutUsIntroductionResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<AboutUsIntroductionResponseDto>> UpdateAsync(
            AboutUsIntroductionRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<AboutUsIntroductionResponseDto>.Failed(result.Errors);
        }

        // Fetch for the entity
        AboutUsIntroduction introduction = await _context.AboutUsIntroductions.SingleAsync();

        // Update photo when the request indicates that it has been changed
        if (requestDto.MainPhotoChanged)
        {
            // Delete the old photo if exists
            if (introduction.MainPhotoUrl != null)
            {
                _photoService.Delete(introduction.MainPhotoUrl);
                introduction.MainPhotoUrl = null;
            }

            // Create a new photo if the request contains the data for a new one
            if (requestDto.MainPhotoFile != null)
            {
                ServiceResult<string> photoServiceResult;
                photoServiceResult = await _photoService.CreateAsync(
                    requestDto.MainPhotoFile,
                    "about-us",
                    false);
                introduction.MainPhotoUrl = photoServiceResult.ResponseDto;
            }
        }

        // Update the other properties
        introduction.MainQuoteContent = requestDto.MainQuoteContent;
        introduction.AboutUsContent = requestDto.AboutUsContent;
        introduction.WhyChooseUsContent = requestDto.WhyChooseUsContent;
        introduction.OurDifferenceContent = requestDto.OurDifferenceContent;
        introduction.OurCultureContent = requestDto.OurCultureContent;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity
        return ServiceResult<AboutUsIntroductionResponseDto>.Success(
            new AboutUsIntroductionResponseDto
            {
                MainPhotoUrl = introduction.MainPhotoUrl,
                MainQuoteContent = introduction.MainQuoteContent,
                AboutUsContent = introduction.AboutUsContent,
                WhyChooseUsContent = introduction.WhyChooseUsContent,
                OurDifferenceContent = introduction.OurDifferenceContent,
                OurCultureContent = introduction.OurCultureContent
            });
    }
}