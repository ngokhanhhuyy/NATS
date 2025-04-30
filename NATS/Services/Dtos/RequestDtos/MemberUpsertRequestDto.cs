namespace NATS.Services.Dtos.RequestDtos;

public class MemberUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string ThumbnailUrl { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }

    public void TransformValues()
    {
        ThumbnailUrl = ThumbnailUrl.ToNullIfEmpty();
        FullName = FullName.ToNullIfEmpty();
        RoleName = RoleName.ToNullIfEmpty();
        Description = Description.ToNullIfEmpty();
    }
}