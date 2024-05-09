using Bonum.Contracts.Interfaces;
using Bonum.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Bonum.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly IOcrService _ocrService;

    public OcrController(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    [HttpGet]
    public async Task<IActionResult> GetImageText([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (!OcrConstants.AllowedContentTypes.Contains(file.ContentType))
            return BadRequest("Invalid content type");

        if (file.Length > FileConstants.TenMbInBytes)
            return BadRequest("File length is greater than 10 MB");

        var response = await _ocrService.GetTextFromImage(file.OpenReadStream(), cancellationToken);
        return Ok(response);
    }
}