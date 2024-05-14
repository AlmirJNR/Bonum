using System.Net.Http.Headers;

namespace Bonum.Tests.Helpers;

public static class MultipartFormDataHelper
{
    public static ByteArrayContent CreateByteArrayContent(MemoryStream stream, string contentType)
    {
        var fileByteContent = new ByteArrayContent(stream.ToArray());
        fileByteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

        return fileByteContent;
    }
}