namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class ServiceController : AbstractCatalogItemController
{
    public ServiceController(
            ICatalogItemService service,
            IValidator<CatalogItemUpsertRequestDto> validator)
        : base(CatalogItemType.Service, service, validator)
    {
    }
}