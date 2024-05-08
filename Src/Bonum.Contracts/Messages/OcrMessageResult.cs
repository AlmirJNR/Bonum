namespace Bonum.Contracts.Messages;

public class OcrMessageResult
{
    public string ImageText { get; private init; }

    public OcrMessageResult(string imageText)
    {
        ImageText = imageText;
    }
}