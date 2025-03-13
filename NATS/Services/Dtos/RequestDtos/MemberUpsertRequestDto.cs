namespace NATS.Services.Dtos.RequestDtos;

public class MemberUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public byte[] ThumbnailFile { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public bool ThumbnailChanged { get; set; } = false;

    public void TransformValues()
    {
        FullName = FullName.ToNullIfEmpty();
        RoleName = RoleName.ToNullIfEmpty();
        Description = Description.ToNullIfEmpty();
    }
}