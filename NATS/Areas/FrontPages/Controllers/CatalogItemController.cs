using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/")]
public class CatalogItemController : Controller
{
    private readonly ICatalogItemService _service;

    public CatalogItemController(ICatalogItemService service)
    {
        _service = service;
    }

    [HttpGet("dich-vu")]
    public async Task<IActionResult> ServiceList()
    {
        return await CatalogItemList(CatalogItemType.Service);
    }

    [HttpGet("dich-vu/{id:int}")]
    public async Task<IActionResult> ServiceDetail(int id)
    {
        return await CatalogItemDetail(id, CatalogItemType.Service);
    }

    [HttpGet("khoa-hoc")]
    public async Task<IActionResult> CourseList()
    {
        return await CatalogItemList(CatalogItemType.Course);
    }

    [HttpGet("khoa-hoc/{id:int}")]
    public async Task<IActionResult> CourseDetail(int id)
    {
        return await CatalogItemDetail(id, CatalogItemType.Course);
    }

    [HttpGet("san-pham")]
    public async Task<IActionResult> ProductList()
    {
        return await CatalogItemList(CatalogItemType.Product);
    }

    [HttpGet("san-pham/{id:int}")]
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
                ExcludedIds = new List<int> { id }
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