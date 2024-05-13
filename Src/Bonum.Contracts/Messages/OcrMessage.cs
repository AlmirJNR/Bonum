namespace Bonum.Contracts.Messages;

public class OcrMessage
{
    public byte[] File { get; private init; }
    public string[] Languages { get; private init; }

    public OcrMessage(byte[] file, string[] languages)
    {
        File = file;
        Languages = languages.Length == 0
            ? new[] { "eng", "por" }
            : languages;
    }
}