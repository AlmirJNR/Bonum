namespace Bonum.Tests.Helpers;

public static class FileHelper
{
    public static FileInfo GetFileInfo(string filePath)
    {
        var file = new FileInfo(filePath);
        return file;
    }

    public static async Task<MemoryStream> CopyFileToMemory(FileInfo file)
    {
        var memoryStream = new MemoryStream();
        await file.OpenRead().CopyToAsync(memoryStream);
        return memoryStream;
    }

    public static string GetFileContentType(in FileInfo file)
    {
        return file.Extension switch
        {
            ".png" => "image/png",
            _ => throw new ArgumentOutOfRangeException(nameof(file), file.Extension)
        };
    }
}