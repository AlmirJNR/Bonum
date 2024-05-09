using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using Bonum.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Bonum.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly IAmqpClient<OcrMessage, OcrMessageResult> _ocrClient;

    public OcrController(IAmqpClient<OcrMessage, OcrMessageResult> ocrClient)
    {
        _ocrClient = ocrClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetImageText([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (!OcrConstants.AllowedContentTypes.Contains(file.ContentType))
            return BadRequest("Invalid content type");

        if (file.Length > FileConstants.TenMbInBytes)
            return BadRequest("File length is greater than 10 MB");

        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();
        var response = await _ocrClient.Request(new OcrMessage(fileBytes), cancellationToken);
        return Ok(response);
    }
}