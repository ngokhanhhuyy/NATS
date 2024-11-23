namespace NATS.Models;

public class TeamMemberListViewModel
{
    [Display(Name = DisplayNames.TeamMembers)]
    public List<TeamMemberViewModel> TeamMembers { get; set; }

    [Display(Name = DisplayNames.BusinessCertificates)]
    public List<BusinessCertificateViewModel> BusinessCertificates { get; set; }
}