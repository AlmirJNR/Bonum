using System.Diagnostics;
using Bonum.Contracts.Messages;
using MassTransit;

namespace Bonum.Ocr.Consumers;

public class OcrConsumer : IConsumer<OcrMessage>
{
    public async Task Consume(ConsumeContext<OcrMessage> context)
    {
        var tempImageFilePath = Path.GetTempFileName();
        await File.WriteAllBytesAsync(tempImageFilePath, context.Message.File);

        using var ocrProcess = Process.Start(new ProcessStartInfo
        {
            Arguments = $"{tempImageFilePath} - -l {string.Join('+', context.Message.Languages)} quiet",
            CreateNoWindow = true,
            FileName = "tesseract",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
        });

        if (ocrProcess is null)
            throw new NullReferenceException("OCR process was not started");

        await ocrProcess.WaitForExitAsync();
        if (ocrProcess.ExitCode != 0)
        {
            var stoutError = await ocrProcess.StandardError.ReadToEndAsync();
            throw new Exception(stoutError);
        }

        var ocrText = (await ocrProcess.StandardOutput.ReadToEndAsync()).Trim();
        await context.RespondAsync(new OcrMessageResult(ocrText));
        File.Delete(tempImageFilePath);
    }
}