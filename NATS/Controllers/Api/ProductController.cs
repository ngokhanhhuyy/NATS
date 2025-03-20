namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class ProductController : AbstractCatalogItemController
{
    public ProductController(
            ICatalogItemService service,
            IValidator<CatalogItemUpsertRequestDto> validator)
        : base(CatalogItemType.Course, service, validator)
    {
    }
}