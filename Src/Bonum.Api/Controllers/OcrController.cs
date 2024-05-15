using Bonum.Contracts.Dtos;
using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using Bonum.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Bonum.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OcrController : ControllerBase
{
    private readonly IOcrService _ocrService;

    public OcrController(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OcrMessageResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetImageText(
        [FromForm] WrapperDto<IFormFile> file,
        [FromForm] string[] languages,
        CancellationToken cancellationToken
    )
    {
        if (file.Value is null)
            return BadRequest("Invalid file");

        if (!OcrConstants.AllowedContentTypes.Contains(file.Value.ContentType))
            return BadRequest("Invalid content type");

        if (file.Value.Length > FileConstants.TenMbInBytes)
            return BadRequest("File length is greater than 10 MB");

        var response = await _ocrService.GetTextFromImage(file.Value.OpenReadStream(), cancellationToken, languages);
        if (string.IsNullOrWhiteSpace(response.Text))
            return NoContent();

        return Ok(response);
    }
}