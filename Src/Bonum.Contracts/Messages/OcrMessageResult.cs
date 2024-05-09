namespace Bonum.Contracts.Messages;

public class OcrMessageResult
{
    public string Text { get; private init; }

    public OcrMessageResult(string text)
    {
        Text = text;
    }
}