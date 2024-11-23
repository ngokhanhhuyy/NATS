namespace NATS.Models;

public class BusinessServiceFeatureViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Content)]
    public string Content { get; set; }

    public bool IsDeleted { get; set; } = false;
}