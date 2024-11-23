namespace NATS.Services.Dtos.RequestDtos;

public class BusinessServicePhotoRequestDto : IRequestDto<BusinessServicePhotoRequestDto>
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool IsDeleted { get; set; }

    public BusinessServicePhotoRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        return this;
    }
}
