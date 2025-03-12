namespace NATS.Models;

public class RoleDetailModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public RoleDetailModel(RoleResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        DisplayName = responseDto.DisplayName;
    }
}