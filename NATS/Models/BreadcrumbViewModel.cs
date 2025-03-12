namespace NATS.Models;

public class BreadcrumbModel
{
    public List<(string DisplayName, string Url)> Items { get; set; }
}