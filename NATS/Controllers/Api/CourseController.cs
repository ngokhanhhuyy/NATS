namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class CourseController : AbstractCatalogItemController
{
    public CourseController(
            ICatalogItemService service,
            IValidator<CatalogItemUpsertRequestDto> validator)
        : base(CatalogItemType.Course, service, validator)
    {
    }
}