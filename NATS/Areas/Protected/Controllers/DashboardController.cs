using NATS.Protected.Models;

namespace NATS.Protected.Controllers;

[Area("Protected")]
[Route("quan-tri")]
[Authorize]
public class DashboardController : Controller
{
    private readonly ITrafficService _trafficService;

    public const string DashboardRouteName = "ProtectedDashboard";

    public DashboardController(ITrafficService trafficService)
    {
        _trafficService = trafficService;
    }

    [HttpGet(Name = DashboardRouteName)]
    public IActionResult Index()
    {
        return View();
    }
}