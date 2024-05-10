namespace Bonum.Contracts.Interfaces;

public interface IAmqpClient<TMessage, TResult>
{
    public Task<TResult> Request(TMessage message, CancellationToken cancellationToken);
}