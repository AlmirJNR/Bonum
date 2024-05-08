namespace Bonum.Contracts.Messages;

public class OcrMessage
{
    public byte[] File { get; private init; }
    public string[] Languages { get; set; } = { "eng", "por" };

    public OcrMessage(byte[] file)
    {
        File = file;
    }
}