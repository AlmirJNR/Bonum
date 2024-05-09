using Bonum.Contracts.Messages;

namespace Bonum.Contracts.Interfaces;

public interface IOcrService
{
    public Task<OcrMessageResult> GetTextFromImage(Stream stream, CancellationToken cancellationToken);
}