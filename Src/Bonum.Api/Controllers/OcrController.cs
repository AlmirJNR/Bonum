using Bonum.Contracts.Dtos;
using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
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

    [ProducesResponseType(typeof(OcrMessageResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("image")]
    public async Task<IActionResult> GetImageText(
        [FromForm] WrapperDto<IFormFile> file,
        CancellationToken cancellationToken
    )
    {
        if (file.Value is null)
            return BadRequest("Invalid file");

        if (!OcrConstants.AllowedImageContentTypes.Contains(file.Value.ContentType))
            return BadRequest("Invalid content type");

        if (file.Value.Length > FileConstants.TenMbInBytes)
            return BadRequest("File length is greater than 10 MB");

        var response = await _ocrService.GetTextFromImage(file.Value.OpenReadStream(), cancellationToken);
        if (string.IsNullOrWhiteSpace(response.Text))
            return NoContent();

        return Ok(response);
    }
}