namespace NATS.Models;

public class BreadcrumbViewModel
{
    public List<(string DisplayName, string Url)> Items { get; set; }
}