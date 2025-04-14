using NATS.Public.Models;

namespace NATS.Public.Controllers;

[Area("Public")]
[Route("/")]
public class CatalogItemController : Controller
{
    private readonly ICatalogItemService _service;

    public const string ServiceListRouteName = "PublicServiceList";
    public const string ServiceDetailRouteName = "PublicServiceDetail";
    public const string CourseListRouteName = "PublicCourseList";
    public const string CourseDetailRouteName = "PublicCourseDetail";
    public const string ProductListRouteName = "PublicProductList";
    public const string ProductDetailRouteName = "PublicProductDetail";

    public CatalogItemController(ICatalogItemService service)
    {
        _service = service;
    }

    [HttpGet("dich-vu", Name = ServiceListRouteName)]
    public async Task<IActionResult> ServiceList()
    {
        return await CatalogItemList(CatalogItemType.Service);
    }

    [HttpGet("dich-vu/{id:int}", Name = ServiceDetailRouteName)]
    public async Task<IActionResult> ServiceDetail(int id)
    {
        return await CatalogItemDetail(id, CatalogItemType.Service);
    }

    [HttpGet("khoa-hoc", Name = CourseListRouteName)]
    public async Task<IActionResult> CourseList()
    {
        return await CatalogItemList(CatalogItemType.Course);
    }

    [HttpGet("khoa-hoc/{id:int}", Name = CourseDetailRouteName)]
    public async Task<IActionResult> CourseDetail(int id)
    {
        return await CatalogItemDetail(id, CatalogItemType.Course);
    }

    [HttpGet("san-pham", Name = ProductListRouteName)]
    public async Task<IActionResult> ProductList()
    {
        return await CatalogItemList(CatalogItemType.Product);
    }

    [HttpGet("san-pham/{id:int}", Name = ProductDetailRouteName)]
    public async Task<IActionResult> ProductDetail(int id)
    {
        return await CatalogItemDetail(id, CatalogItemType.Product);
    }

    private async Task<IActionResult> CatalogItemList(CatalogItemType type)
    {
        List<CatalogItemBasicResponseDto> responseDtos;
        responseDtos = await _service.GetListAsync(new CatalogItemListRequestDto
        {
            Type = type
        });

        CatalogItemListViewModel model = new CatalogItemListViewModel(responseDtos, type);

        return View("CatalogItemList", model);
    }

    private async Task<IActionResult> CatalogItemDetail(int id, CatalogItemType type)
    {
        try
        {
            CatalogItemDetailResponseDto detailResponseDto;
            detailResponseDto = await _service.GetDetailAsync(id);
            if (detailResponseDto.Type != type)
            {
                return RedirectToAction(type.ToString() + "List");
            }

            List<CatalogItemBasicResponseDto> otherResponseDtos;
            otherResponseDtos = await _service.GetListAsync(new CatalogItemListRequestDto
            {
                Type = type,
            });

            CatalogItemDetailViewModel model = new CatalogItemDetailViewModel(
                detailResponseDto,
                otherResponseDtos);

            return View("CatalogItemDetail", model);
        }
        catch (ResourceNotFoundException)
        {
            return RedirectToAction(type.ToString() + "List");
        }
    }
}