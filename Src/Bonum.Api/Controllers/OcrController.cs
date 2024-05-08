using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Bonum.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    /// 10mb in bytes
    private const int FileSizeLimit = 10485760;

    private static readonly string[] AllowedContentType =
    {
        "image/avif",
        "image/jpeg",
        "image/jpg",
        "image/jpeg",
        "image/jfif",
        "image/pjpeg",
        "image/pjp",
        "image/png"
    };

    private readonly IAmqpClient<OcrMessage, OcrMessageResult> _ocrClient;

    public OcrController(IAmqpClient<OcrMessage, OcrMessageResult> ocrClient)
    {
        _ocrClient = ocrClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetImageText([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (!AllowedContentType.Contains(file.ContentType))
            return BadRequest("Invalid content type");

        if (file.Length > FileSizeLimit)
            return BadRequest("File length is greater than 10 MB");

        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();
        var response = await _ocrClient.Request(new OcrMessage(fileBytes), cancellationToken);
        return Ok(response);
    }
}