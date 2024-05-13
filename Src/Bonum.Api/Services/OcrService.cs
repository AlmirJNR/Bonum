using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;

namespace Bonum.Api.Services;

public class OcrService : IOcrService
{
    private readonly IAmqpClient<OcrMessage, OcrMessageResult> _ocrClient;

    public OcrService(IAmqpClient<OcrMessage, OcrMessageResult> ocrClient)
    {
        _ocrClient = ocrClient;
    }

    public async Task<OcrMessageResult> GetTextFromImage(
        Stream stream,
        CancellationToken cancellationToken,
        params string[] languages
    )
    {
        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();
        var response = await _ocrClient.Request(new OcrMessage(fileBytes, languages), cancellationToken);
        return response;
    }
}