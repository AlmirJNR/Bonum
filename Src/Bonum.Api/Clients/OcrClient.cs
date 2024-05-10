using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using MassTransit;

namespace Bonum.Api.Clients;

public class OcrClient : IAmqpClient<OcrMessage, OcrMessageResult>
{
    private readonly IRequestClient<OcrMessage> _client;

    public OcrClient(IRequestClient<OcrMessage> client)
    {
        _client = client;
    }

    public async Task<OcrMessageResult> Request(OcrMessage message, CancellationToken cancellationToken)
    {
        var response = await _client.GetResponse<OcrMessageResult>(message, cancellationToken);
        return response.Message;
    }
}