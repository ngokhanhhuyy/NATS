namespace NATS.Models;

public class UserBasicViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int TimeZone { get; set; }
    public RoleViewModel Role { get; set; }
}