using Microsoft.AspNetCore.Mvc.Rendering;

namespace NATS.Models;

public class GeneralSettingsViewModel
{
    [Display(Name = DisplayNames.ApplicationName)]
    [Required]
    [MaxLength(100)]
    public string ApplicationName { get; set; }

    [Display(Name = DisplayNames.ApplicationShortName)]
    [Required]
    [StringLength(15)]
    public string ApplicationShortName { get; set; }

    [Display(Name = DisplayNames.FavIcon)]
    public string FavIconUrl { get; set; }

    [Display(Name = DisplayNames.FavIcon)]
    public byte[] FavIconFile { get; set; }

    [Display(Name = DisplayNames.UnderMaintainance)]
    public bool UnderMaintainance { get; set; }

    [BindNever]
    public List<SelectListItem> UnderMaintainanceOptions => new List<SelectListItem>
    {
        new SelectListItem
        {
            Value = "false",
            Text = "Đang hoạt động",
            Selected = !UnderMaintainance
        },
        new SelectListItem
        {
            Value = "true",
            Text = "Đang bảo trì",
            Selected = UnderMaintainance
        }
    };
}
