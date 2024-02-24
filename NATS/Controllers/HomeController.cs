namespace NATS.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _context;

    public HomeController(DatabaseContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/gioi-thieu")]
    public IActionResult AboutUs()
    {
        return View("AboutUs");
    }

    [HttpGet("/doi-ngu")]
    public IActionResult TeamMembers()
    {
        return View("TeamMembers");
    }

    [HttpGet("/dao-tao")]
    public IActionResult Education()
    {
        return View("Education");
    }

    [HttpGet("/dao-tao/khoa-hoc")]
    public IActionResult CourseList()
    {
        return View("CourseList");
    }

    [HttpGet("/dich-vu")]
    public IActionResult BusinessServiceList()
    {
        return View("BusinessServiceList");
    }

    [HttpGet("/san-pham")]
    public IActionResult ProductList()
    {
        return View("ProductList");
    }
}
