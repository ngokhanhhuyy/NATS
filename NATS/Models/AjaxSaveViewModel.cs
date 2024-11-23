namespace NATS.Models;

public class AjaxSaveViewModel
{
    public string FormId { get; set; }
    public string RedirectingUrl { get; set; }
    public bool EditableAfterSaving { get; set; }
    public string ButtonText { get; set; } = "Lưu";
}