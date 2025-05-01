using System.Net.Http.Headers;

namespace NATS.Controllers;

[Authorize]
[Area("Protected")]
[Route("tai-len")]
public class ImageUploadController : Controller
{
    // [HttpPost]
    // public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
    // {
    //     if (image == null || image.Length == 0)
    //     {
    //         return BadRequest();
    //     }
    //     
    //     using MultipartFormDataContent formDataContent = new MultipartFormDataContent();
    //     await using Stream imageStream = image.OpenReadStream();
    //     StreamContent imageContent = new StreamContent(imageStream);
    //     imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
    //     formDataContent.Add(imageContent, "image", image.FileName);
    //
    //     using HttpClient httpClient = new HttpClient();
    //     string key = "5766dd450acc191f87589a57db07f429";
    //     string url = $"https://api.imgbb.com/1/upload?key={key}";
    //     HttpResponseMessage responseMessage = await httpClient.PostAsync(url, formDataContent);
    // }
}